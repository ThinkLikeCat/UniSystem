# Баги к исправлению (UniSystem)

## 1. [CRITICAL] Admin reset-password → 500 Internal Server Error

**Путь:** `UniSystem.Application/Admin/Commands/ResetPassword/ResetPasswordCommand.cs:25-26`

**Симптом:** `POST /api/admin/users/{id}/reset-password` возвращает 500.

**Код:**
```csharp
var token = await _userManager.GeneratePasswordResetTokenAsync(user);
var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);
```

**План:**
- Заменить на `RemovePasswordAsync` + `AddPasswordAsync` (не требует DataProtection)
- Можно оставить `GeneratePasswordResetTokenAsync` если это тестовое окружение
- Проверить логи сервера для точной диагностики

---

## 2. [HIGH] Куратор видит ВСЕ заявления

**Путь:** `UniSystem.Application/Documents/Queries/GetDocumentsQuery.cs:43-44`

**Симптом:** Куратор видит документы всех студентов, а должен — только своей группы.

**Код:**
```csharp
if (_userContext.Roles.Contains("StudentProfile"))
    query = query.Where(d => d.AuthorId == userId);
// Остальные роли не фильтруются
```

**План:**
- Добавить фильтр для Curator по `StaffProfile.AcademicGroupId`
- Найти всех студентов группы куратора → показать только их документы
- Опционально: применить аналогичный фильтр для `StaffProfile`

---

## 3. [HIGH] Студент сам меняет себе группу через профиль

**Путь:** `UniSystem.Application/Users/Commands/UpdateStudentProfile/UpdateStudentProfileCommand.cs`
**Фронтенд:** `frontend/app/(authenticated)/profile/edit/page.tsx`

**Симптом:** На странице `/profile/edit` студент может изменить:
- `academicGroupId` (группа)
- `studentStatusId` (статус)
- `studentTicket` (номер студенческого)

Эти поля должны задаваться только администратором.

**План:**
- Убрать `AcademicGroupId`, `StudentStatusId` из `UpdateStudentProfileCommand`
- Оставить только `StudentTicket` (возможно, тоже убрать — на усмотрение)
- Убрать соответствующие `<select>` из фронтенда профиля студента
- `StudentTicket` — возможно, тоже убрать (назначается админом)

---

## 4. [HIGH] Админ не может изменить группу студента при редактировании

**Путь (бэкенд):** `UniSystem.Application/Admin/Commands/UpdateUser/UpdateUserCommand.cs`
**Путь (фронтенд):** `frontend/app/(authenticated)/admin/users/[id]/page.tsx`

**Симптом:** При создании пользователя админ может задать группу, а при редактировании — нет. Форма редактирования содержит только: `firstName`, `lastName`, `patronymic`, `sex`, `role`.

**План (бэкенд):**
- Добавить в `UpdateUserCommand` поля:
  ```csharp
  string? StudentTicket = null,
  int? AcademicGroupId = null,
  int? StudentStatusId = null,
  int? DepartmentId = null
  ```
- В `UpdateUserCommandHandler` добавить обновление `StudentProfile`/`StaffProfile` при наличии полей:
  ```csharp
  if (request.StudentTicket is not null)
      _context.Entry(user.StudentProfile).Property("StudentTicket").CurrentValue = request.StudentTicket;
  ```

**План (фронтенд):**
- На странице `/admin/users/[id]` добавить секцию "Профиль" с полями в зависимости от роли:
  - **Student:** StudentTicket, AcademicGroupId, StudentStatusId
  - **Staff/Dean/Secretary/Curator:** DepartmentId, AcademicGroupId (curated)
  - **Admin:** ничего

---

## 5. [MEDIUM] Смена роли не создаёт целевой профиль

**Путь:** `UniSystem.Application/Admin/Commands/UpdateUser/UpdateUserCommand.cs:61-68`

**Симптом:** При смене роли со Student на Staff:
- Старый `StudentProfile` **не удаляется** (если новая роль не Admin)
- Новый `StaffProfile` **не создаётся**
- Пользователь остаётся без профиля

**План:**
- При смене роли:
  - Если `targetRole == Admin`: удалить оба профиля (уже есть)
  - Если `targetRole == StudentProfile`: создать `StudentProfile` если нет; удалить `StaffProfile` если есть (админ должен указать ticket/group/status)
  - Если `targetRole` staff/curator/dean/secretary: создать `StaffProfile` если нет; удалить `StudentProfile` если есть
- Валидировать обязательные поля для новой роли (DepartmentId для staff, StudentTicket/AcademicGroupId/StudentStatusId для student)
- Поля для профиля должны передаваться вместе с `Role` в `UpdateUserCommand`

---

## 6. [LOW] Обновить AGENTS.md после исправлений

После завершения исправлений обновить `AGENTS.md` с актуальной информацией:
- Исправленные баги
- Новые поля в UpdateUserCommand
- Изменённые роли и их permission'ы

---

## Порядок выполнения

| № | Задача | Зависит от |
|---|--------|-----------|
| 1 | Исправить admin reset-password 500 | — |
| 2 | Добавить профильные поля в UpdateUserCommand (бэкенд) | — |
| 3 | Обновить админ-панель — поля группы/статуса (фронтенд) | 2 |
| 4 | Убрать группу/статус из саморедактирования студента | 2 |
| 5 | Добавить фильтрацию документов для куратора | — |
| 6 | Исправить создание профилей при смене роли | 2 |

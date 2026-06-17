# UniSystem — University Document Management System

Clean Architecture + Domain-Driven Design. Система управления документами студентов университета.

---

## Technology Stack

| Технология | Версия |
|------------|--------|
| .NET | 10.0 |
| EF Core | 10.0.8 |
| PostgreSQL (Npgsql) | 10.0.2 |
| MediatR | 14.1.0 |
| Swashbuckle (Swagger) | 10.1.4 |
| EFCore.NamingConventions | 10.0.1 |

---

## Solution Structure

```
UniSystem.sln
└── src/
    ├── UniSystem.Domain/          # Domain layer
    ├── UniSystem.Application/     # Application layer
    ├── UniSystem.Infrastructure/  # Infrastructure layer
    └── UniSystem.Web/             # Presentation layer
```

---

## UniSystem.Domain — Domain Layer

DDD-слой: сущности, Value Objects, enum'ы, доменные исключения. Не имеет внешних зависимостей.

```
UniSystem.Domain/
├── Common/
│   ├── Entity.cs                   # Базовый класс Entity<TId> с Equals/GetHashCode
│   └── IEntityId.cs                # Интерфейс идентификатора (Guid Value)
├── Entities/
│   ├── AcademicGroup.cs            # Учебная группа (backing field _students)
│   ├── Department.cs               # Кафедра
│   ├── Document.cs                 # Документ (статусы, шаблон, резолюция)
│   ├── DocumentAttachment.cs       # Прикреплённый файл
│   ├── DocumentStatus.cs           # Статус документа
│   ├── DocumentType.cs             # Тип документа (шаблон текста)
│   ├── Role.cs                     # Роль пользователя
│   ├── Specialty.cs                # Специальность
│   ├── StaffProfile.cs             # Профиль сотрудника
│   ├── StaffSubject.cs             # Связь сотрудник-предмет (composite key)
│   ├── StudentProfile.cs           # Профиль студента
│   ├── StudentStatus.cs            # Статус студента
│   ├── Subject.cs                  # Учебный предмет
│   └── User.cs                     # Пользователь (FullName computed)
├── Enums/
│   ├── Sex.cs                      # Male / Female / Other
│   └── SystemRoleName.cs           # StaffProfile / StudentProfile
├── ValueObjects/
│   ├── Identifiers.cs              # UserId, StudentId, StaffId, DocumentId, AttachmentId
│   ├── AcademicGroup/
│   │   ├── Course.cs               # short (1-6)
│   │   ├── GroupMaxCount.cs        # int (0-35)
│   │   └── GroupName.cs            # string (max 20)
│   ├── Department/
│   │   └── DepartmentName.cs       # string (max 150)
│   ├── DocumentAttachment/
│   │   ├── FileName.cs             # string (max 255, валидация расширения)
│   │   ├── FilePath.cs             # string (max 500, проверка символов)
│   │   └── FileSize.cs             # long (>= 0)
│   ├── DocumentStatus/
│   │   └── DocumentStatusName.cs   # string (max 50)
│   ├── DocumentType/
│   │   ├── DocumentTypeName.cs     # string (max 100)
│   │   └── TemplateText.cs         # проверка плейсхолдеров, Tokenize()
│   ├── Role/
│   │   ├── RoleNameDative.cs       # string (max 55)
│   │   └── RoleNameNominative.cs   # string (max 50)
│   ├── Specialty/
│   │   ├── SpecialtyCode.cs        # string (max 20)
│   │   ├── SpecialtyDuration.cs    # short (3-6)
│   │   └── SpecialtyName.cs        # string (max 150)
│   ├── StudentStatus/
│   │   └── StatusName.cs           # string (max 50)
│   ├── Subject/
│   │   └── SubjectName.cs          # string (max 150)
│   └── User/
│       ├── Email.cs                # string (max 255, regex)
│       ├── FirstName.cs            # string (max 50)
│       ├── IconPath.cs             # string (max 255, проверка символов)
│       ├── LastName.cs             # string (max 55)
│       ├── PasswordHash.cs         # string (не пустой)
│       └── Patronymic.cs           # string (max 60, nullable)
└── Exceptions/
    ├── DomainException.cs          # Базовое доменное исключение
    ├── AcademicGroupExceptions.cs  # Ошибки группы (переполнение, дубликат и т.д.)
    ├── DepartmentExceptions.cs     # Ошибки кафедры
    ├── DocumentAttachmentExceptions.cs
    ├── DocumentExceptions.cs
    ├── DocumentStatusExceptions.cs
    ├── DocumentTypeExceptions.cs   # Ошибки шаблона (плейсхолдеры)
    ├── RoleExceptions.cs
    ├── SpecialtyExceptions.cs
    ├── StaffProfileExceptions.cs
    ├── StaffSubjectExceptions.cs
    ├── StudentProfileExceptions.cs
    ├── StudentStatusExceptions.cs
    ├── SubjectExceptions.cs
    └── UserExceptions.cs
```

---

## UniSystem.Application — Application Layer

CQRS-слой (MediatR установлен). Пока содержит заглушку.

```
UniSystem.Application/
├── Class1.cs                          # Placeholder
└── UniSystem.Application.csproj       # Зависимость: UniSystem.Domain; MediatR 14.1.0
```

---

## UniSystem.Infrastructure — Infrastructure Layer

EF Core DbContext, миграции PostgreSQL, Value Converters.

```
UniSystem.Infrastructure/
├── UniSystemDbContext.cs              # Контекст БД (Fluent API, 26 Value Converters)
├── UniSystemDbContextFactory.cs       # IDesignTimeDbContextFactory для миграций
├── Migrations/
│   ├── 20260616191441_InitialCreate.cs           # Создание всех 14 таблиц
│   ├── 20260616191441_InitialCreate.Designer.cs
│   ├── 20260616193957_AutoPendingChange.cs        # Cascade delete + уникальные индексы
│   ├── 20260616193957_AutoPendingChange.Designer.cs
│   └── UniSystemDbContextModelSnapshot.cs
└── UniSystem.Infrastructure.csproj    # Зависимости: UniSystem.Application, UniSystem.Domain
                                       # EF Core 10.0.8, Npgsql 10.0.2,
                                       # EFCore.NamingConventions 10.0.1
```

### Особенности конфигурации БД

- **Snake_case** — имена таблиц и колонок
- **DeleteBehavior.Cascade** — на всех внешних ключах
- **Value Converters** — все Value Object (22 шт.) конвертируются в примитивы
- **Enum → string** — Sex, SystemRoleName хранятся как строки
- **Уникальные индексы** — Email, SystemName, NameNominative, NameDative, Department.Name, Specialty.Name, Specialty.Code, AcademicGroup.Name, StudentStatus.Name, Subject.Name, DocumentType.Name, DocumentStatus.Name
- **Identity (SERIAL)** — Role.Id, Department.Id, Specialty.Id, AcademicGroup.Id, StudentStatus.Id, Subject.Id, DocumentType.Id, DocumentStatus.Id
- **Backing field** — AcademicGroup.Students → `_students`
- **Composite key** — StaffSubject (StaffId, SubjectId)

### Схема БД (14 таблиц)

| Таблица | PK | Сущность |
|---------|----|----------|
| `users` | UUID | User |
| `roles` | SERIAL | Role |
| `student_profiles` | UUID | StudentProfile |
| `staff_profiles` | UUID | StaffProfile |
| `departments` | SERIAL | Department |
| `specialties` | SERIAL | Specialty |
| `academic_groups` | SERIAL | AcademicGroup |
| `student_statuses` | SERIAL | StudentStatus |
| `subjects` | SERIAL | Subject |
| `staff_subjects` | composite (UUID + int) | StaffSubject |
| `documents` | UUID | Document |
| `document_types` | SERIAL | DocumentType |
| `document_statuses` | SERIAL | DocumentStatus |
| `document_attachments` | UUID | DocumentAttachment |

---

## UniSystem.Web — Presentation Layer

ASP.NET Web API хост.

```
UniSystem.Web/
├── Program.cs                        # Точка входа (DI, Swagger, Npgsql, Controllers)
├── appsettings.json                  # Connection string, конфигурация
├── appsettings.Development.json      # Логирование (Development)
├── Properties/
│   └── launchSettings.json           # Профили: Develop (5000/5001), Production (7000/7001)
├── UniSystem.Web.http                # Тестовый запрос к API
└── UniSystem.Web.csproj              # Зависимости: все проекты; Swagger, MediatR, Negotiate
```

### Запуск

```bash
# Настройка connection string в appsettings.json
# Применение миграций
dotnet ef database update --project UniSystem.Infrastructure --startup-project UniSystem.Web

# Запуск (по умолчанию https://localhost:5001)
dotnet run --project UniSystem.Web
```

---

## Связи между проектами

```
UniSystem.Web ──► UniSystem.Application ──► UniSystem.Domain
     │                                         ▲
     └────────── UniSystem.Infrastructure ──────┘
                      (EF Core)
```

- **Domain** — независимый слой (без зависимостей)
- **Application** — зависит от Domain (CQRS-команды/запросы)
- **Infrastructure** — зависит от Application и Domain (реализация persistence)
- **Web** — зависит от всех слоёв (DI composition root)

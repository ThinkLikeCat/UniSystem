# UniSystem — University Document Management System

Clean Architecture + CQRS/MediatR + Domain-Driven Design.  
Система управления документами студентов университета.

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
| ASP.NET Core Identity | 10.0.8 |
| JWT Bearer | 10.0.9 |
| FluentValidation | 12.0 |

---

## Roles (6)

| System Name | Отображаемое имя | Назначение |
|------------|-----------------|------------|
| `StudentProfile` | Студент | Создаёт и отправляет документы |
| `StaffProfile` | Сотрудник | Просмотр документов |
| `Secretary` | Секретарь | Проверка документов (секретарь) |
| `Dean` | Декан | Утверждение/отклонение документов |
| `Curator` | Куратор | Просмотр документов |
| `Admin` | Администратор | Управление пользователями, справочниками, статистика |

---

## API Endpoints (66 total)

### Auth (`api/auth`)
| Method | Route | Auth | Описание |
|--------|-------|------|----------|
| POST | `/register` | Admin | Создать пользователя |
| POST | `/login` | Public | Вход → JWT |
| GET | `/me` | All | Текущий пользователь |

### Profile (`api/profile`)
| Method | Route | Auth | Описание |
|--------|-------|------|----------|
| GET | `/` | All | Профиль |
| PUT | `/student` | Student | Обновить профиль студента |
| PUT | `/staff` | Staff/Dean/Secretary/Curator | Обновить профиль сотрудника |
| POST | `/change-password` | All | Сменить пароль |

### Documents (`api/documents`)
| Method | Route | Auth | Описание |
|--------|-------|------|----------|
| POST | `/` | Student | Создать черновик |
| GET | `/` | All | Список (студент — только свои) |
| GET | `/{id}` | All | Детально |
| PUT | `/{id}/dynamic-values` | Student | Редактировать поля (черновик/доработка) |
| POST | `/{id}/send-to-review` | Student | Отправить на проверку |
| POST | `/{id}/secretary-review` | Secretary | Решение секретаря |
| POST | `/{id}/dean-review` | Dean | Решение декана |

### Attachments (`api/documents/{documentId}/attachments`)
| Method | Route | Auth | Описание |
|--------|-------|------|----------|
| POST | `/` | Student | Загрузить файл |
| GET | `/{attachmentId}` | All | Скачать |
| DELETE | `/{attachmentId}` | Student | Удалить |

### Справочники (7 entities × 5 endpoints)
| Controller | Route | POST/PUT/DELETE |
|-----------|-------|-----------------|
| Departments | `api/departments` | Admin |
| Specialties | `api/specialties` | Admin |
| AcademicGroups | `api/academic-groups` | Admin |
| DocumentTypes | `api/document-types` (GET `/{id}/template`) | Admin |
| DocumentStatuses | `api/document-statuses` | Admin |
| StudentStatuses | `api/student-statuses` | Admin |
| Subjects | `api/subjects` | Admin |

### StaffSubjects (`api/staff-subjects`)
| Method | Route | Auth |
|--------|-------|------|
| GET | `/{staffId}` | All |
| POST | `/` | Admin |
| DELETE | `/{staffId}/{subjectId}` | Admin |

### Admin (`api/admin`)
| Method | Route | Описание |
|--------|-------|----------|
| GET | `/users?roleFilter=` | Список пользователей |
| GET | `/users/{id}` | Детально пользователь |
| POST | `/users` | Создать пользователя |
| PUT | `/users/{id}` | Обновить пользователя |
| DELETE | `/users/{id}` | Удалить пользователя |
| POST | `/users/{id}/reset-password` | Сбросить пароль |
| GET | `/statistics/summary` | Сводка по статусам |
| GET | `/statistics/by-month` | По месяцам |
| GET | `/statistics/by-type` | По типам документов |
| GET | `/statistics/resolution` | С резолюцией / без |
| GET | `/statistics/by-status` | По текущему статусу |

---

## Document Workflow

```
Черновик ──отправить──► На проверке секретаря ──одобрить──► На проверке декана ──утвердить──► Утверждён
                             │                                                  └──отклонить──► Отклонён
                             ├──на доработку──► Черновик (цикл)
                             └──отклонить─────► Отклонён
```

---

## Solution Structure

```
UniSystem.sln
├── UniSystem.Domain/           # Entities, ValueObjects, Enums, Exceptions (~60 files)
├── UniSystem.Application/      # CQRS Commands/Handlers, Queries, Validators (~60 files)
├── UniSystem.Infrastructure/   # DbContext, JwtProvider, SeedData, Migrations
└── UniSystem.Web/              # Controllers (13), Middleware, Program.cs
```

---

## Database

- **Provider**: PostgreSQL
- **Naming**: snake_case (EFCore.NamingConventions)
- **Migration**: 1 InitialCreate (squashed), Identity tables + 14 domain tables
- **22 Value Converters** — все Value Object конвертируются в примитивы
- **Enum → string** — Sex, SystemRoleName
- **Unique indexes** — Email, SystemName, NameNominative, NameDative, все названия справочников

### Tables (14 domain + 6 Identity)

| Таблица | PK | Сущность |
|---------|----|----------|
| `users` | UUID | User |
| `roles` | UUID | Role |
| `student_profiles` | UUID | StudentProfile |
| `staff_profiles` | UUID | StaffProfile |
| `departments` | SERIAL | Department |
| `specialties` | SERIAL | Specialty |
| `academic_groups` | SERIAL | AcademicGroup |
| `student_statuses` | SERIAL | StudentStatus |
| `subjects` | SERIAL | Subject |
| `staff_subjects` | composite | StaffSubject |
| `documents` | UUID | Document |
| `document_types` | SERIAL | DocumentType |
| `document_statuses` | SERIAL | DocumentStatus |
| `document_attachments` | UUID | DocumentAttachment |
| +6 Identity tables | | UserRoles, RoleClaims, UserClaims, UserLogins, UserTokens |

---

## Key Design Decisions

- **Value Objects as PKs** — DocumentId, AttachmentId (readonly record struct)
- **EF Core private setter bypass** — `_context.Entry(entity).Property("Prop").CurrentValue = val`
- **Shared PK** — StudentProfile.Id = StaffProfile.Id = AspNetUsers.Id
- **JWT** — ClaimTypes.NameIdentifier + ClaimTypes.Role
- **Validation** — FluentValidation pipeline behavior (ValidationException → HTTP 400)
- **Domain exceptions** → HTTP 400 via ExceptionHandlingMiddleware
- **Student document filter** — студент видит только свои документы (role-based)
- **Decision statuses** — DocumentSecretaryStatusId / DocumentDeanStatusId фиксируются

---

## Seed Data

### Default Admin
- Email: `admin@unisystem.local`
- Password: `Admin123!`

### Document Statuses
1. Черновик
2. На проверке секретаря
3. На доработку
4. На проверке декана
5. Утверждён
6. Отклонён

---

## Build & Run

```bash
# Build
dotnet build .\UniSystem.sln
# Always: 0 errors, 0 warnings

# Apply migrations
dotnet ef database update --project UniSystem.Infrastructure --startup-project UniSystem.Web

# Run
dotnet run --project UniSystem.Web
```

---

## Build Status

✅ **0 errors, 0 warnings** — стабильная сборка

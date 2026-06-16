using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UniSystem.Domain.Entities;
using UniSystem.Domain.Enums;
using UniSystem.Domain.ValueObjects;
using UniSystem.Domain.ValueObjects.AcademicGroup;
using UniSystem.Domain.ValueObjects.Department;
using UniSystem.Domain.ValueObjects.DocumentAttachment;
using UniSystem.Domain.ValueObjects.DocumentStatus;
using UniSystem.Domain.ValueObjects.DocumentType;
using UniSystem.Domain.ValueObjects.Role;
using UniSystem.Domain.ValueObjects.Specialty;
using UniSystem.Domain.ValueObjects.StudentStatus;
using UniSystem.Domain.ValueObjects.Subject;
using UniSystem.Domain.ValueObjects.User;

namespace UniSystem.Infrastructure;

public class UniSystemDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<StudentProfile> StudentProfiles => Set<StudentProfile>();
    public DbSet<StaffProfile> StaffProfiles => Set<StaffProfile>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Specialty> Specialties => Set<Specialty>();
    public DbSet<AcademicGroup> AcademicGroups => Set<AcademicGroup>();
    public DbSet<StudentStatus> StudentStatuses => Set<StudentStatus>();
    public DbSet<Subject> Subjects => Set<Subject>();
    public DbSet<StaffSubject> StaffSubjects => Set<StaffSubject>();
    public DbSet<Document> Documents => Set<Document>();
    public DbSet<DocumentType> DocumentTypes => Set<DocumentType>();
    public DbSet<DocumentStatus> DocumentStatuses => Set<DocumentStatus>();
    public DbSet<DocumentAttachment> DocumentAttachments => Set<DocumentAttachment>();

    public UniSystemDbContext(DbContextOptions<UniSystemDbContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSnakeCaseNamingConvention();
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<UserId>().HaveConversion<UserIdConverter>();
        configurationBuilder.Properties<DocumentId>().HaveConversion<DocumentIdConverter>();
        configurationBuilder.Properties<AttachmentId>().HaveConversion<AttachmentIdConverter>();

        configurationBuilder.Properties<Email>().HaveConversion<EmailConverter>();
        configurationBuilder.Properties<PasswordHash>().HaveConversion<PasswordHashConverter>();
        configurationBuilder.Properties<FirstName>().HaveConversion<FirstNameConverter>();
        configurationBuilder.Properties<LastName>().HaveConversion<LastNameConverter>();
        configurationBuilder.Properties<Patronymic>().HaveConversion<PatronymicConverter>();
        configurationBuilder.Properties<IconPath>().HaveConversion<IconPathConverter>();
        configurationBuilder.Properties<GroupName>().HaveConversion<GroupNameConverter>();
        configurationBuilder.Properties<GroupMaxCount>().HaveConversion<GroupMaxCountConverter>();
        configurationBuilder.Properties<Course>().HaveConversion<CourseConverter>();
        configurationBuilder.Properties<DepartmentName>().HaveConversion<DepartmentNameConverter>();
        configurationBuilder.Properties<SpecialtyName>().HaveConversion<SpecialtyNameConverter>();
        configurationBuilder.Properties<SpecialtyCode>().HaveConversion<SpecialtyCodeConverter>();
        configurationBuilder.Properties<SpecialtyDuration>().HaveConversion<SpecialtyDurationConverter>();
        configurationBuilder.Properties<StatusName>().HaveConversion<StatusNameConverter>();
        configurationBuilder.Properties<SubjectName>().HaveConversion<SubjectNameConverter>();
        configurationBuilder.Properties<RoleNameNominative>().HaveConversion<RoleNameNominativeConverter>();
        configurationBuilder.Properties<RoleNameDative>().HaveConversion<RoleNameDativeConverter>();
        configurationBuilder.Properties<DocumentStatusName>().HaveConversion<DocumentStatusNameConverter>();
        configurationBuilder.Properties<DocumentTypeName>().HaveConversion<DocumentTypeNameConverter>();
        configurationBuilder.Properties<TemplateText>().HaveConversion<TemplateTextConverter>();
        configurationBuilder.Properties<FileName>().HaveConversion<FileNameConverter>();
        configurationBuilder.Properties<FilePath>().HaveConversion<FilePathConverter>();
        configurationBuilder.Properties<FileSize>().HaveConversion<FileSizeConverter>();

        configurationBuilder.Properties<Sex>().HaveConversion<string>();
        configurationBuilder.Properties<SystemRoleName>().HaveConversion<string>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.Email).IsUnique();

            entity.HasOne(e => e.Role)
                .WithMany()
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.StudentProfile)
                .WithOne(e => e.User)
                .HasForeignKey<StudentProfile>(e => e.Id)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.StaffProfile)
                .WithOne(e => e.User)
                .HasForeignKey<StaffProfile>(e => e.Id)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<StudentProfile>(entity =>
        {
            entity.Property(e => e.StudentTicket).HasMaxLength(50);

            entity.HasOne(e => e.AcademicGroup)
                .WithMany(e => e.Students)
                .HasForeignKey(e => e.AcademicGroupId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.StudentStatus)
                .WithMany()
                .HasForeignKey(e => e.StudentStatusId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<StaffProfile>(entity =>
        {
            entity.HasOne(e => e.Department)
                .WithMany()
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.AcademicGroup)
                .WithMany()
                .HasForeignKey(e => e.AcademicGroupId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasIndex(e => e.SystemName).IsUnique();
            entity.HasIndex(e => e.NameNominative).IsUnique();
            entity.HasIndex(e => e.NameDative).IsUnique();
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasIndex(e => e.Name).IsUnique();
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<Specialty>(entity =>
        {
            entity.HasIndex(e => e.Name).IsUnique();
            entity.HasIndex(e => e.Code).IsUnique();
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<AcademicGroup>(entity =>
        {
            entity.HasIndex(e => e.Name).IsUnique();
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.HasOne(e => e.Specialty)
                .WithMany()
                .HasForeignKey(e => e.SpecialtyId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Navigation(e => e.Students)
                .HasField("_students");
        });

        modelBuilder.Entity<StudentStatus>(entity =>
        {
            entity.HasIndex(e => e.Name).IsUnique();
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasIndex(e => e.Name).IsUnique();
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<StaffSubject>(entity =>
        {
            entity.HasKey(e => new { e.StaffId, e.SubjectId });

            entity.HasOne(e => e.StaffProfile)
                .WithMany()
                .HasForeignKey(e => e.StaffId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Subject)
                .WithMany()
                .HasForeignKey(e => e.SubjectId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.Property(e => e.DynamicValues).HasMaxLength(4000);
            entity.Property(e => e.ResolutionComment).HasMaxLength(2000);

            entity.HasOne(e => e.Student)
                .WithMany()
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.DocumentType)
                .WithMany()
                .HasForeignKey(e => e.DocumentTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.CurrentStatus)
                .WithMany()
                .HasForeignKey(e => e.DocumentCurrentStatusId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.SecretaryStatus)
                .WithMany()
                .HasForeignKey(e => e.DocumentSecretaryStatusId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.DeanStatus)
                .WithMany()
                .HasForeignKey(e => e.DocumentDeanStatusId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.ResolvedByUser)
                .WithMany()
                .HasForeignKey(e => e.ResolvedByUserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.Attachments)
                .WithOne(e => e.Document)
                .HasForeignKey(e => e.DocumentId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<DocumentType>(entity =>
        {
            entity.HasIndex(e => e.Name).IsUnique();
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<DocumentStatus>(entity =>
        {
            entity.HasIndex(e => e.Name).IsUnique();
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<DocumentAttachment>(entity =>
        {
            entity.HasOne(e => e.Document)
                .WithMany(e => e.Attachments)
                .HasForeignKey(e => e.DocumentId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private sealed class UserIdConverter : ValueConverter<UserId, Guid>
    {
        public UserIdConverter() : base(v => v.Value, v => new UserId(v)) { }
    }

    private sealed class DocumentIdConverter : ValueConverter<DocumentId, Guid>
    {
        public DocumentIdConverter() : base(v => v.Value, v => new DocumentId(v)) { }
    }

    private sealed class AttachmentIdConverter : ValueConverter<AttachmentId, Guid>
    {
        public AttachmentIdConverter() : base(v => v.Value, v => new AttachmentId(v)) { }
    }

    private sealed class EmailConverter : ValueConverter<Email, string>
    {
        public EmailConverter() : base(v => v.Value, v => new Email(v)) { }
    }

    private sealed class PasswordHashConverter : ValueConverter<PasswordHash, string>
    {
        public PasswordHashConverter() : base(v => v.Value, v => new PasswordHash(v)) { }
    }

    private sealed class FirstNameConverter : ValueConverter<FirstName, string>
    {
        public FirstNameConverter() : base(v => v.Value, v => new FirstName(v)) { }
    }

    private sealed class LastNameConverter : ValueConverter<LastName, string>
    {
        public LastNameConverter() : base(v => v.Value, v => new LastName(v)) { }
    }

    private sealed class PatronymicConverter : ValueConverter<Patronymic, string>
    {
        public PatronymicConverter() : base(v => v.Value, v => new Patronymic(v)) { }
    }

    private sealed class IconPathConverter : ValueConverter<IconPath, string>
    {
        public IconPathConverter() : base(v => v.Value, v => new IconPath(v)) { }
    }

    private sealed class GroupNameConverter : ValueConverter<GroupName, string>
    {
        public GroupNameConverter() : base(v => v.Value, v => new GroupName(v)) { }
    }

    private sealed class GroupMaxCountConverter : ValueConverter<GroupMaxCount, int>
    {
        public GroupMaxCountConverter() : base(v => v.Value, v => new GroupMaxCount(v)) { }
    }

    private sealed class CourseConverter : ValueConverter<Course, short>
    {
        public CourseConverter() : base(v => v.Value, v => new Course(v)) { }
    }

    private sealed class DepartmentNameConverter : ValueConverter<DepartmentName, string>
    {
        public DepartmentNameConverter() : base(v => v.Value, v => new DepartmentName(v)) { }
    }

    private sealed class SpecialtyNameConverter : ValueConverter<SpecialtyName, string>
    {
        public SpecialtyNameConverter() : base(v => v.Value, v => new SpecialtyName(v)) { }
    }

    private sealed class SpecialtyCodeConverter : ValueConverter<SpecialtyCode, string>
    {
        public SpecialtyCodeConverter() : base(v => v.Value, v => new SpecialtyCode(v)) { }
    }

    private sealed class SpecialtyDurationConverter : ValueConverter<SpecialtyDuration, short>
    {
        public SpecialtyDurationConverter() : base(v => v.Value, v => new SpecialtyDuration(v)) { }
    }

    private sealed class StatusNameConverter : ValueConverter<StatusName, string>
    {
        public StatusNameConverter() : base(v => v.Value, v => new StatusName(v)) { }
    }

    private sealed class SubjectNameConverter : ValueConverter<SubjectName, string>
    {
        public SubjectNameConverter() : base(v => v.Value, v => new SubjectName(v)) { }
    }

    private sealed class RoleNameNominativeConverter : ValueConverter<RoleNameNominative, string>
    {
        public RoleNameNominativeConverter() : base(v => v.Value, v => new RoleNameNominative(v)) { }
    }

    private sealed class RoleNameDativeConverter : ValueConverter<RoleNameDative, string>
    {
        public RoleNameDativeConverter() : base(v => v.Value, v => new RoleNameDative(v)) { }
    }

    private sealed class DocumentStatusNameConverter : ValueConverter<DocumentStatusName, string>
    {
        public DocumentStatusNameConverter() : base(v => v.Value, v => new DocumentStatusName(v)) { }
    }

    private sealed class DocumentTypeNameConverter : ValueConverter<DocumentTypeName, string>
    {
        public DocumentTypeNameConverter() : base(v => v.Value, v => new DocumentTypeName(v)) { }
    }

    private sealed class TemplateTextConverter : ValueConverter<TemplateText, string>
    {
        public TemplateTextConverter() : base(v => v.Value, v => new TemplateText(v)) { }
    }

    private sealed class FileNameConverter : ValueConverter<FileName, string>
    {
        public FileNameConverter() : base(v => v.Value, v => new FileName(v)) { }
    }

    private sealed class FilePathConverter : ValueConverter<FilePath, string>
    {
        public FilePathConverter() : base(v => v.Value, v => new FilePath(v)) { }
    }

    private sealed class FileSizeConverter : ValueConverter<FileSize, long>
    {
        public FileSizeConverter() : base(v => v.Value, v => new FileSize(v)) { }
    }
}
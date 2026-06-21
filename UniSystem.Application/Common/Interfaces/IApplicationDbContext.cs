using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using UniSystem.Domain.Entities;

namespace UniSystem.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<StudentProfile> StudentProfiles { get; }
    DbSet<StaffProfile> StaffProfiles { get; }
    DbSet<Department> Departments { get; }
    DbSet<Specialty> Specialties { get; }
    DbSet<AcademicGroup> AcademicGroups { get; }
    DbSet<StudentStatus> StudentStatuses { get; }
    DbSet<Subject> Subjects { get; }
    DbSet<StaffSubject> StaffSubjects { get; }
    DbSet<Document> Documents { get; }
    DbSet<DocumentType> DocumentTypes { get; }
    DbSet<DocumentStatus> DocumentStatuses { get; }
    DbSet<DocumentAttachment> DocumentAttachments { get; }

    EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

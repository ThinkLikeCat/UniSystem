using UniSystem.Domain.Common;
using UniSystem.Domain.ValueObjects;

namespace UniSystem.Domain.Entities;

public class StaffProfile : Entity<StaffId>
{
    public int DepartmentId { get; private set; }
    public Department Department { get; private set; } = null!;

    public int? AcademicGroupId { get; private set; } = null;
    public AcademicGroup? AcademicGroup { get; private set; }

    public User User { get; private set; } = null!;

    protected StaffProfile() { }
    
    public StaffProfile(StaffId id, int departmentId, int? academicGroupId) : base(id)
    {
        if (departmentId <= 0)
            throw new ArgumentException("Невалидный ID типа отделения.", nameof(departmentId));
        
        if(academicGroupId is not null && academicGroupId <= 0)
            throw new ArgumentException("Невалидный ID группы.", nameof(academicGroupId));
        
        DepartmentId = departmentId;
        AcademicGroupId = academicGroupId;
    }
}
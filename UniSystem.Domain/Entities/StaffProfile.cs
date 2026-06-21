using UniSystem.Domain.Common;
using UniSystem.Domain.Exceptions;

namespace UniSystem.Domain.Entities;

public class StaffProfile : Entity<Guid>
{
    public int DepartmentId { get; private set; }
    public Department Department { get; private set; } = null!;

    public int? AcademicGroupId { get; private set; } = null;
    public AcademicGroup? AcademicGroup { get; private set; }

    public User User { get; private set; } = null!;

    internal void SetUser(User user) => User = user;
    
    protected StaffProfile() { }
    
    internal StaffProfile(Guid id, int departmentId, int? academicGroupId) : base(id)
    {
        if (departmentId <= 0)
            throw new InvalidStaffDepartmentReferenceException(departmentId);
        
        if(academicGroupId is not null && academicGroupId <= 0)
            throw new InvalidStaffAcademicGroupReferenceException(academicGroupId);
        
        DepartmentId = departmentId;
        AcademicGroupId = academicGroupId;
    }
}
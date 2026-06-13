using UniSystem.Domain.Common;
using UniSystem.Domain.ValueObjects;

namespace UniSystem.Domain.Entities;

public class StaffProfile : Entity<StaffId>
{
    public StaffProfile(StaffId id) : base(id) { }
    protected StaffProfile() { }

    public int DepartmentId { get; set; }
    public Department Department { get; set; } = null!;

    public int? AcademicGroupId { get; set; } = null;
    public AcademicGroup? AcademicGroup { get; set; }

    public User User { get; set; } = null!;
}
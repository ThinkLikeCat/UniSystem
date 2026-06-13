using UniSystem.Domain.ValueObjects;

namespace UniSystem.Domain.Entities;

public class StaffSubject
{
    public StaffId StaffId { get; set; }
    public StaffProfile StaffProfile { get; set; }

    public int SubjectId { get; set; }
    public Subject Subject { get; set; }
}
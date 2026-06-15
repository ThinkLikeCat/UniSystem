using UniSystem.Domain.ValueObjects;

namespace UniSystem.Domain.Entities;

public class StaffSubject
{
    public StaffId StaffId { get; private set; }
    public StaffProfile StaffProfile { get; private set; } = null!;

    public int SubjectId { get; private set; }
    public Subject Subject { get; private set; } = null!;

    protected StaffSubject() { }

    public StaffSubject(StaffId staffId, int subjectId)
    {
        if (subjectId <= 0)
            throw new ArgumentException("Невалидный ID предмета.", nameof(subjectId));
        
        StaffId = staffId;
        SubjectId = subjectId;
    }
}
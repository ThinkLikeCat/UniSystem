using UniSystem.Domain.Common;
using UniSystem.Domain.ValueObjects.Id;

namespace UniSystem.Domain.Entities;

public class UniversitySubject: Entity<UniversitySubjectId>
{
    public string Name { get; private set; } = string.Empty;
    public TeacherId TeacherId { get; private set; }
    public bool IsCompleted { get; private set; } = false;
    public int CourseStudyStarting { get; private set; } = 1;
    
    public void CompleteSubject() => IsCompleted = true;
}
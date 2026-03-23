using UniSystem.Domain.ValueObjects;

namespace UniSystem.Domain.Entities;

public class UniversitySubject
{
    public UniversitySubjectId Id { get; private set; } = UniversitySubjectId.Empty();
    
    public string Name { get; private set; } = string.Empty;
    public Teacher Teacher { get; private set; } = new();
    
    public bool IsCompleted { get; private set; } = false;
    public int CourseStudyStarting { get; private set; } = 1;

    public UniversitySubject() {}
    
    public UniversitySubject(string name, Teacher teacher, int courseStudyStarting)
    {
        Id = UniversitySubjectId.NewId();
        Name = name.Trim();
        Teacher = teacher;
        CourseStudyStarting = courseStudyStarting;
    }
    
    public void CompleteSubject() => IsCompleted = true;
}
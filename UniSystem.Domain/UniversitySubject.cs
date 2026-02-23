namespace UniSystem.Domain;

public class UniversitySubject
{
    public Guid Id { get; private set; }
    
    public string Name { get; private set; }
    public Teacher Teacher { get; private set; }
    public bool IsCompleted { get; private set; }
    public int CourseStudyStarting { get; private set; }
}
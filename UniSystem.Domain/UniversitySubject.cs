namespace UniSystem.Domain;

public class UniversitySubject
{
    public string Name { get; private set; }
    public Teacher Teacher { get; private set; }
    
    public bool IsCompleted { get; private set; } = false;
    public int CourseStudyStarting { get; private set; }

    public UniversitySubject(string name, Teacher teacher, int courseStudyStarting)
    {
        Name = name;
        Teacher = teacher;
        CourseStudyStarting = courseStudyStarting;
    }
    
    public void CompleteSubject() => IsCompleted = true;
}
 namespace UniSystem.Domain;

public class UniversityGroup
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    
    public string Name { get; private set; }
    public Curator Curator { get; private set; }
    public List<Student> Students { get; private set; }
    public List<UniversitySubject> LearningSubjects { get; private set; }
    public List<UniversitySubject> EndedSubjects { get; private set; }
    
    public UniversityGroup(string name, Curator curator, List<Student> students,
        List<UniversitySubject> learningSubjects, List<UniversitySubject>? endedSubjects = null)
    {
        Name = name;
        Curator = curator;
        Students = students;
        LearningSubjects = learningSubjects;
        EndedSubjects = endedSubjects ?? new List<UniversitySubject>();
    }

    public void ReplaceCurator(Curator curator) => Curator = curator;
    
    /*public void ReplaceStudentGroup(Student student, UniversityGroup newUniversityGroup)
    {
        if (student is null || newUniversityGroup is null) 
            throw new ArgumentNullException();
        
        if (Students.Remove(student))
        {
            student.UniversityGroup = newUniversityGroup;
        }
    }*/
}
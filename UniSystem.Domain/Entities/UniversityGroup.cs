using UniSystem.Domain.ValueObjects;
namespace UniSystem.Domain.Entities;

public class UniversityGroup
{
    public UniversityGroupId Id { get; private set; } = UniversityGroupId.Empty();
    
    public string Name { get; private set; } = string.Empty;
    public Speciality Speciality { get; private set; } = new();
    public Curator Curator { get; private set; } = new();
    public List<Student> Students { get; private set; } = new();
    public List<UniversitySubject> LearningSubjects { get; private set; } = new();
    public List<UniversitySubject> EndedSubjects { get; private set; } = new();

    public UniversityGroup() {}

    public UniversityGroup(string name, Speciality speciality, Curator curator, List<Student> students,
        List<UniversitySubject> learningSubjects, List<UniversitySubject>? endedSubjects = null)
    {
        Id = UniversityGroupId.NewId();
        Name = name;
        Speciality = speciality;
        Curator = curator;
        Students = students;
        LearningSubjects = learningSubjects;
        EndedSubjects = endedSubjects ?? new List<UniversitySubject>();
    }

    public void AddStudent(Student student)
    {
        if (student.Speciality == Speciality) Students.Add(student);
        else throw new ArgumentException();
    }
}
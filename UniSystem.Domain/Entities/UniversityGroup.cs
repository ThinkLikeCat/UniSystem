using UniSystem.Domain.Entities.Common;
using UniSystem.Domain.ValueObjects;

namespace UniSystem.Domain.Entities;

public class UniversityGroup: Entity<UniversityGroupId>
{
    public string Name { get; private set; } = string.Empty;
    public Speciality Speciality { get; private set; } = new();
    public Curator Curator { get; private set; } = new();

    public int StudentsMaxCount
    {
        get;
        private set => field = value is > 15 and < 50
            ? value
            : throw new ArgumentOutOfRangeException(nameof(StudentsMaxCount));
    } = 30;

    public List<Student> Students { get; private set; } = new();
    public List<UniversitySubject> LearningSubjects { get; private set; } = new();
    public List<UniversitySubject> EndedSubjects { get; private set; } = new();

    public UniversityGroup() {}

    public UniversityGroup(string name, Speciality speciality, Curator curator, int studentsMaxCount, 
        List<Student> students, List<UniversitySubject> learningSubjects, List<UniversitySubject>? endedSubjects = null)
    {
        Name = name;
        Speciality = speciality;
        Curator = curator;
        StudentsMaxCount = studentsMaxCount;
        AddStudents(students);
        LearningSubjects = learningSubjects;
        EndedSubjects = endedSubjects ?? new List<UniversitySubject>();
    }

    public void AddStudent(Student student)
    {
        if(Students.Count >= StudentsMaxCount)
            throw new ArgumentOutOfRangeException(nameof(StudentsMaxCount));

        if (Students.All(x => x.Id != student.Id))
        {
            student.TransferToAnotherUniversityGroup(this);
            Students.Add(student);
        }
    }

    public void AddStudents(List<Student> students)
    {
        if(students.Count + Students.Count > StudentsMaxCount)
            throw new ArgumentOutOfRangeException(nameof(StudentsMaxCount));

        foreach (var student in students)
        {
            AddStudent(student);
        }
    }
}
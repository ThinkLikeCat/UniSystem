using UniSystem.Domain.Common;
using UniSystem.Domain.ValueObjects.Id;

namespace UniSystem.Domain.Entities;

public class UniversityGroup: Entity<UniversityGroupId>
{
    public string Name { get; private set; }
    public SpecialityId SpecialityId { get; private set; }
    public TeacherId CuratorId { get; private set; }
    public int Course { get; private set; }

    public int StudentsMaxCount
    {
        get;
        private set => field = value is > 15 and < 50
            ? value
            : throw new Exception(nameof(StudentsMaxCount));
    } = 30;

    public List<StudentId> StudentsId { get; private set; } = new();
    public List<UniversitySubjectId> LearningSubjectsId { get; private set; } = new();
    public List<UniversitySubjectId> EndedSubjectsId { get; private set; } = new();

    public void AddStudent(Student student)
    {
        if (StudentsId.Any(id => id == student.Id))
            return;
        
        if(StudentsId.Count == StudentsMaxCount)
            throw new Exception(nameof(StudentsMaxCount));

        StudentsId.Add(student.Id);
        student.SetGroupAndSpeciality(Id, SpecialityId);
    }

    public void AddStudents(List<Student> students)
    {
        if(students.Count + StudentsId.Count > StudentsMaxCount)
            throw new Exception(nameof(StudentsMaxCount));

        foreach (var student in students)
        {
            AddStudent(student);
        }
    }
}
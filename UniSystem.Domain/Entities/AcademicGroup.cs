using UniSystem.Domain.ValueObjects.AcademicGroup;

namespace UniSystem.Domain.Entities;

using Exceptions;

public class AcademicGroup
{
    public int Id { get; private set; }

    public GroupName Name { get; private set; } = null!;
    public GroupMaxCount MaxCount { get; private set; } = null!;
    public Course Course { get; private set; } = null!;
    
    public int SpecialtyId { get; private set; }
    public Specialty Specialty { get; private set; } = null!;

    private readonly List<StudentProfile> _students = new();
    public IReadOnlyCollection<StudentProfile> Students => _students.AsReadOnly();

    public int CurrentCount => _students.Count;

    protected AcademicGroup() { }

    public AcademicGroup(string name, int maxCount, short course, int specialtyId)
    {
        if (specialtyId <= 0)
            throw new InvalidSpecialtyReferenceException(specialtyId);
        
        Name = new GroupName(name);
        MaxCount = new GroupMaxCount(maxCount);
        SpecialtyId = specialtyId;
        Course = new Course(course);
    }

    public void AddStudent(StudentProfile student)
    {
        if(Students.Any(s => s.Id == student.Id))
            throw new StudentAlreadyInGroupException(student.Id, Name.Value);
        
        if (!MaxCount.CanAccommodate(CurrentCount))
            throw new GroupIsFullException(Name.Value);

        _students.Add(student);
    }

    public void RemoveStudent(StudentProfile student)
    {
        if (student is null)
            throw new ArgumentNullException(nameof(student), "Студент не может быть null.");
        
        if (!_students.Contains(student))
            throw new StudentNotInGroupException(student.Id, Name.Value);
        
        _students.Remove(student);
    }
}
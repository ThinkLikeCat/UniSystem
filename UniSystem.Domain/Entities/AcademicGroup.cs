namespace UniSystem.Domain.Entities;

using Exceptions;

public class AcademicGroup
{
    const int MinAllowed = 0;
    const int MaxAllowed = 35;
    
    public int Id { get; private set; }

    public string Name { get; private set; } = string.Empty;
    public int MaxCount { get; private set; }

    public int SpecialtyId { get; private set; }
    public Specialty Specialty { get; private set; } = null!;

    private readonly List<StudentProfile> _students = new();
    public IReadOnlyCollection<StudentProfile> Students => _students.AsReadOnly();

    public int CurrentCount => _students.Count;

    protected AcademicGroup() { }

    public AcademicGroup(string name, int maxCount, int specialtyId)
    {
        ChangeName(name);

        SetMaxCount(maxCount);

        if (specialtyId <= 0)
            throw new DomainException("Указан невалидный Id специальности.");

        SpecialtyId = specialtyId;
    }

    public void ChangeName(string groupName)
    {
        if (string.IsNullOrWhiteSpace(groupName))
            throw new InvalidAcademicGroupNameException("Имя группы не может быть пустым.");

        var cleanedName = groupName.Trim();

        if (cleanedName.Length > 20)
            throw new InvalidAcademicGroupNameException(cleanedName);

        Name = cleanedName;
    }

    public void SetMaxCount(int count)
    {
        if (count < MinAllowed || count > MaxAllowed)
            throw new GroupSizeOutOfRangeException(count, MinAllowed, MaxAllowed);

        if (CurrentCount > count)
            throw new DomainException($"New max count ({count}) cannot be less than current count ({CurrentCount}).");

        MaxCount = count;
    }

    public void AddStudent(StudentProfile student)
    {
        if (CurrentCount >= MaxCount)
            throw new DomainException($"Группа {Name} переполнена.");

        _students.Add(student);
    }

    public void RemoveStudent(StudentProfile student)
    {
        if (student is null)
            throw new ArgumentNullException(nameof(student), "Студент не может быть null.");
        
        if (!_students.Contains(student))
            throw new DomainException($"Студент с ID {student.Id} не принадлежит группе {Name}.");
        
        _students.Remove(student);
    }
}
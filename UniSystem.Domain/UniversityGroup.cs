using System.Collections.Concurrent;

namespace UniSystem.Domain;

public class UniversityGroup
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    
    public string Name { get; private set; }
    public Curator Curator { get; private set; }
    public List<Student> Students { get; private set; }
    
    public UniversityGroup(string name, Curator curator, List<Student> students)
    {
        Name = name;
        Curator = curator;
        Students = students;
    }

    public void AddStudent(Student student) => Students.Add(student);
    
    public void RemoveStudent(Student student) => Students.Remove(student);

    public void ReplaceStudent(ref Student student, ref Student newStudent, ref UniversityGroup newUniversityGroup)
    {
        Students.Add(newStudent);
        Students.Remove(student);
        newUniversityGroup.Students.Add(newStudent);
    }

    public void ReplaceCurator(Curator curator) => Curator = curator;
}
namespace UniSystem.Domain.Entities;

public class Curator: Teacher
{
    public UniversityGroup SupervisesGroup { get; private set; } = new();
    
    public void AddStudent(Student student) => SupervisesGroup.Students.Add(student);
    
    public void RemoveStudent(Student student) => SupervisesGroup.Students.Remove(student);
}
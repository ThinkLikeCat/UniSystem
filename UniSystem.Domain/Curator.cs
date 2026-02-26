namespace UniSystem.Domain;

public class Curator: Member
{
    public UniversityGroup SupervisesGroup { get; private set; }
    
    public void AddStudent(Student student) => SupervisesGroup.Students.Add(student);
    
    public void RemoveStudent(Student student) => SupervisesGroup.Students.Remove(student);
}
namespace UniSystem.Domain.Entities;

public class Curator: Teacher
{
    public UniversityGroup SupervisesGroup { get; private set; } = new();
    
    public Curator() {}

    public void AddStudent(Student student)
    {
        if (student.Speciality != SupervisesGroup.Speciality && student.Speciality is not null)
            throw new ArgumentException();
        else if(student.Speciality is null)
            student.Speciality = SupervisesGroup.Speciality;
        
        SupervisesGroup.Students.Add(student);
        student.UniversityGroup = SupervisesGroup;
    }

    public void RemoveStudent(Student student)
    {
        SupervisesGroup.Students.Remove(student);
        student.UniversityGroup = null;
    }
}
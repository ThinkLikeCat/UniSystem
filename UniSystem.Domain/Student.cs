namespace UniSystem.Domain;

public class Student: Member
{
    public Guid Id { get; private set; }
    public UniversityGroup UniversityGroup { get; private set; }
    public Speciality Speciality { get; private set; }
    public int Course { get; private set; }
    public string Email { get; private set; }
    public string State {get; private set;}
    public bool IsCompletedEducation { get; private set; }

    public void NextYearTransfer()
    {
        if (IsCompletedEducation)
            throw new ArgumentOutOfRangeException($"Student {FullName} has completed education");
        
        Course++;
        
        if(Course > Speciality.MaxCourse)
            IsCompletedEducation = true;
    }
}
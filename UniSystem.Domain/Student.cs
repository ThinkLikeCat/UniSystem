namespace UniSystem.Domain;

public class Student: Member
{
    public string Email { get; private set; }
    
    public Speciality Speciality { get; private set; }
    public int Course { get; private set; }
    public UniversityGroup UniversityGroup { get; private set; }

    public string State { get; private set; } = string.Empty;
    public bool IsCompletedEducation { get; private set; } = false;

    public void NextYearTransfer()
    {
        if (IsCompletedEducation)
            throw new ArgumentOutOfRangeException($"Student {FullName} has completed education");
        
        Course++;
        
        if (Course > Speciality.MaxCourse)
            IsCompletedEducation = true;
    }
}
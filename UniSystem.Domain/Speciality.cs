namespace UniSystem.Domain;

public class Speciality
{
    public Guid Id { get; private set; }

    public string Title { get; private set; }
    public string Description { get; private set; }
    public int MaxCourse{ get; private set; }
    
    public List<UniversitySubject> ActiveUniversitySubjects { get; private set; }
    public List<UniversitySubject> ClosedUniversitySubjects { get; private set; }

    public Speciality(string title, string description, int maxCourse, List<UniversitySubject> universitySubjects)
    {
        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        MaxCourse = maxCourse;
        ActiveUniversitySubjects = universitySubjects;
        ClosedUniversitySubjects = new List<UniversitySubject>();
    }

    public void CloseUniversitySubject(ref UniversitySubject universitySubject)
    {
        if (ActiveUniversitySubjects.Remove(universitySubject))
            ClosedUniversitySubjects.Add(universitySubject);
    }
}
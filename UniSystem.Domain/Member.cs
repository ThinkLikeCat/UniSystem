namespace UniSystem.Domain;

public abstract class Member
{
    public Guid Id { get; private set; } =  Guid.NewGuid();
    
    public string FullName { get; private set; }
    public DateTime BirthDate { get; private set; }
    
    public string Password { get; private set; }
    
    public int Age
    {
        get
        {
            var dateRightNow = DateTime.Now;
            var age = dateRightNow.Year - BirthDate.Year;
            if (BirthDate.AddYears(age) > dateRightNow) age--;
            return age;
        }
    }

    public string FormatedName
    {
        get
        {
            var nameComponents = FullName.Split();
            return nameComponents[0] + nameComponents[1].First() + '.' + nameComponents[2].First() + '.';
        }
    }
}
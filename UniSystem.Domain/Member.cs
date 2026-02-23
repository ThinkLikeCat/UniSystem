namespace UniSystem.Domain;

public class Member
{
    public Guid Id { get; private set; }
    
    public string FullName { get; private set; }
    public DateTime BirthDate { get; private set; }
    
    public int Age
    {
        get
        {
            var dateRightNow = DateTime.Now;
            var age = dateRightNow.Year - BirthDate.Year;
            if (dateRightNow.Month - dateRightNow.Month >= 0 && dateRightNow.Day >= BirthDate.Day) age++;
            return age;
        }
    }

    public string FormatedName
    {
        get
        {
            var nameComponents = FullName.Split();
            return nameComponents[0] + nameComponents[1].First() + '.' + nameComponents[1].First() + '.';
        }
    }
}
namespace UniSystem.Domain.Entities.Common;

public abstract class Member<TId>: Entity<TId>
{
    public string FullName { get; private set; } = string.Empty;

    public DateTime BirthDate { get; private set; } = DateTime.Now.AddYears(-18);
    
    public string Email { get; private set; } = string.Empty;
    
    public string Password { get; private set; } = string.Empty;

    public int Age
    {
        get
        {
            var currentDate = DateTime.Now;
            var age = currentDate.Year - BirthDate.Year;
            if (BirthDate.AddYears(age) > currentDate) age--;
            return age;
        }
    }

    public string FormattedName
    {
        get
        {
            var nameParts = FullName.Split();
            return nameParts.Length switch
            {
                1 => nameParts[0],
                2 => $"{nameParts[0]} {nameParts[1][0]}.",
                3 => $"{nameParts[0]} {nameParts[1][0]}. {nameParts[2][0]}.",
                _ => FullName
            };
        }
    }

    protected Member(){}
    
    protected Member(string fullName, DateTime birthDate, string email, string password)
    {
        FullName = fullName;
        BirthDate = birthDate;
        Email = email;
        Password = password;
    }
}
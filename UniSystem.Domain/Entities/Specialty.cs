namespace UniSystem.Domain.Entities;

public class Specialty
{
    public int Id { get; set; }

    public string Name { get; private set; } = string.Empty;
    public string Code { get; private set; } = string.Empty;
    public short MaxDurationInYears { get; private set; }
    

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Наименование не может быть пустым или состоять только из пробелов.", nameof(name));

        if (name.Length > 150)
            throw new ArgumentException("Наименование не может превышать 150 символов.", nameof(name));

        Name = name.Trim();
    }
    
    public void SetCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Код не может быть пустым.", nameof(code));

        if (code.Length > 20)
            throw new ArgumentException("Код не может превышать 20 символов.", nameof(code));
        
        Code = code.Trim(); 
    }

    public void SetMaxDurationInYears(short maxDurationInYears)
    {
        if (maxDurationInYears < 3 || maxDurationInYears > 6)
            throw new ArgumentException("Максимальная продолжительность обучения должна быть в диапазоне от 3 до 6", nameof(maxDurationInYears));

        MaxDurationInYears = maxDurationInYears;
    }
}
using UniSystem.Domain.Enums;

namespace UniSystem.Domain.Entities;

public class Role
{
    public int Id { get; set; }

    public SystemRoleName SystemName { get; private set; }
    public string NameNominative { get; private set; } = string.Empty;
    public string NameDative { get; private set; } = string.Empty;
    
    public void SetSystemName(SystemRoleName systemName)
    {
        if (!Enum.IsDefined(typeof(SystemRoleName), systemName))
            throw new ArgumentOutOfRangeException(nameof(systemName), systemName, "Указано неопределенное системное имя.");

        SystemName = systemName;
    }
    
    public void SetNameNominative(string nameNominative)
    {
        if (string.IsNullOrWhiteSpace(nameNominative))
            throw new ArgumentException("Имя в именительном падеже не может быть пустым.", nameof(nameNominative));

        if (nameNominative.Length > 50)
            throw new ArgumentException("Имя в именительном падеже не может превышать 50 символов.", nameof(nameNominative));

        NameNominative = nameNominative;
    }
    
    public void SetNameDative(string nameDative)
    {
        if (string.IsNullOrWhiteSpace(nameDative))
            throw new ArgumentException("Имя в дательном падеже не может быть пустым.", nameof(nameDative));

        if (nameDative.Length > 55)
            throw new ArgumentException("Имя в дательном падеже не может превышать 55 символов.", nameof(nameDative));

        NameDative = nameDative;
    }
}
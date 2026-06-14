using UniSystem.Domain.Enums;
using UniSystem.Domain.ValueObjects.Role;

namespace UniSystem.Domain.Entities;

public class Role
{
    public int Id { get; private set; }

    public SystemRoleName SystemName { get; private set; }
    public RoleNameNominative NameNominative { get; private set; } = null!;
    public RoleNameDative NameDative { get; private set; } = null!;
    
    protected Role() { }

    public Role(SystemRoleName systemName, string nameNominative, string nameDative)
    {
        SetSystemName(systemName);
        NameNominative = new RoleNameNominative(nameNominative);
        NameDative = new RoleNameDative(nameDative);
    }
    
    public void SetSystemName(SystemRoleName systemName)
    {
        if (!Enum.IsDefined(typeof(SystemRoleName), systemName))
            throw new ArgumentOutOfRangeException(nameof(systemName), systemName, "Указано неопределенное системное имя.");

        SystemName = systemName;
    }
}
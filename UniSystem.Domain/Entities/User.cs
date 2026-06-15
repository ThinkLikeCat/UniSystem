using UniSystem.Domain.Common;
using UniSystem.Domain.Enums;
using UniSystem.Domain.ValueObjects;
using UniSystem.Domain.ValueObjects.User;

namespace UniSystem.Domain.Entities;

public class User : Entity<UserId>
{
    public Email Email { get; private set; } = null!;
    public PasswordHash PasswordHash { get; private set; } = null!;
    public FirstName FirstName { get; private set; } = null!;
    public LastName LastName { get; private set; } = null!;
    public Patronymic? Patronymic { get; private set; } = null;
    public Sex Sex { get; private set; }
    public IconPath IconPath { get; private set; } = null!;
    
    public int RoleId { get; private set; }
    public Role Role { get; private set; }
    
    public StudentProfile? StudentProfile { get; private set; } = null;
    public StaffProfile? StaffProfile { get; private set; } = null;
    
    protected User() { }

    public User(UserId id, string email, string passwordHash, string firstName, string lastName, string? patronymic,
        Sex sex, int roleId) : base(id)
    {
        if(roleId <= 0)
            throw new ArgumentException("Невалидный ID роли.", nameof(roleId));
        
        Email = new Email(email);
        PasswordHash = new PasswordHash(passwordHash);
        FirstName = new FirstName(firstName);
        LastName = new LastName(lastName);
        Patronymic = patronymic is null ? null : new Patronymic(patronymic);
        SetSex(sex);
        RoleId = roleId;
    }
    
    public string FullName => Patronymic is null
        ? $"{LastName} {FirstName}"
        : $"{LastName} {FirstName} {Patronymic}";
    
    public void SetSex(Sex sex)
    {
        if (!Enum.IsDefined(typeof(Sex), sex))
            throw new ArgumentOutOfRangeException(nameof(sex), sex, "Указано неопределенное значение пола.");
        
        Sex = sex;
    }
}
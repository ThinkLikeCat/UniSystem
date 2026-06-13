using System.Text.RegularExpressions;
using UniSystem.Domain.Common;
using UniSystem.Domain.Enums;
using UniSystem.Domain.ValueObjects;

namespace UniSystem.Domain.Entities;

public class User : Entity<UserId>
{
    public User(UserId id) : base(id) { }
    protected User() { }

    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string? Patronymic { get; private set; }
    public Sex Sex { get; set; }
    public string IconPath { get; set; } = string.Empty;

    public int RolesId { get; set; }
    public Role Role { get; set; }

    public StudentProfile? StudentProfile { get; set; } = null;
    public StaffProfile? StaffProfile { get; set; } = null;
    
    public void SetEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email не может быть пустым.", nameof(email));

        if (email.Length > 255)
            throw new ArgumentException("Email не может превышать 255 символов.", nameof(email));
        
        var emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        if (!Regex.IsMatch(email, emailRegex))
            throw new ArgumentException("Некорректный формат Email.", nameof(email));

        Email = email.Trim().ToLowerInvariant();
    }
    
    public void SetPasswordHash(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Хэш пароля не может быть пустым.", nameof(passwordHash));

        PasswordHash = passwordHash;
    }
    
    public void SetFirstName(string firstName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("Имя не может быть пустым.", nameof(firstName));

        if (firstName.Length > 50)
            throw new ArgumentException("Имя не может превышать 50 символов.", nameof(firstName));

        FirstName = firstName.Trim();
    }
    
    public void SetLastName(string lastName)
    {
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Фамилия не может быть пустым.", nameof(lastName));

        if (lastName.Length > 55)
            throw new ArgumentException("Фамилия не может превышать 55 символов.", nameof(lastName));

        LastName = lastName.Trim();
    }
    
    public void SetPatronymic(string? patronymic)
    {
        if (patronymic.IsWhiteSpace())
            throw new ArgumentException("", nameof(patronymic));

        if (patronymic is not null && patronymic.Length > 60)
            throw new ArgumentException("Отчество не может превышать 60 символов.", nameof(patronymic));

        Patronymic = patronymic?.Trim();
    }
    
    public void SetSex(Sex sex)
    {
        if (!Enum.IsDefined(typeof(Sex), sex))
            throw new ArgumentOutOfRangeException(nameof(sex), sex, "Указано неопределенное значение пола.");

        Sex = sex;
    }
    
    public void SetIconPath(string iconPath)
    {
        if (string.IsNullOrWhiteSpace(iconPath))
            throw new ArgumentException("Путь к иконке не может быть пустым.", nameof(iconPath));

        if (iconPath.Length > 255)
            throw new ArgumentException("Путь к иконке не может превышать 255 символов.", nameof(iconPath));

        IconPath = iconPath.Trim();
    }
}
using Microsoft.AspNetCore.Identity;
using UniSystem.Domain.Enums;
using UniSystem.Domain.Exceptions;
using UniSystem.Domain.ValueObjects.User;

namespace UniSystem.Domain.Entities;

public class User : IdentityUser<Guid>
{
    public FirstName FirstName { get; private set; } = null!;
    public LastName LastName { get; private set; } = null!;
    public Patronymic? Patronymic { get; private set; } = null;
    public Sex Sex { get; private set; }
    public IconPath IconPath { get; private set; } = null!;

    public StudentProfile? StudentProfile { get; private set; } = null;
    public StaffProfile? StaffProfile { get; private set; } = null;

    protected User() { }

    public User(string firstName, string lastName, string? patronymic,
        Sex sex, string iconPath, string email) : base()
    {
        FirstName = new(firstName);
        LastName = new(lastName);
        Patronymic = patronymic is null ? null : new Patronymic(patronymic);
        SetSex(sex);
        IconPath = new(iconPath);
        UserName = email;
        Email = email;
    }

    public string FullName => Patronymic is null
        ? $"{LastName} {FirstName}"
        : $"{LastName} {FirstName} {Patronymic}";

    public void SetFirstName(string firstName)
    {
        FirstName = new FirstName(firstName);
    }

    public void SetLastName(string lastName)
    {
        LastName = new LastName(lastName);
    }

    public void SetPatronymic(string? patronymic)
    {
        Patronymic = patronymic is null ? null : new Patronymic(patronymic);
    }

    public void SetSex(Sex sex)
    {
        if (!Enum.IsDefined(typeof(Sex), sex))
            throw new InvalidUserSexException(sex);

        Sex = sex;
    }

    public StudentProfile CreateStudentProfile(string studentTicket, int academicGroupId, int studentStatusId)
    {
        if (StudentProfile is not null)
            throw new UserAlreadyHasStudentProfileException();
        if (StaffProfile is not null)
            throw new UserAlreadyHasStaffProfileException();

        var profile = new StudentProfile(Id, studentTicket, academicGroupId, studentStatusId);
        profile.SetUser(this);
        StudentProfile = profile;
        return profile;
    }

    public StaffProfile CreateStaffProfile(int departmentId, int? academicGroupId)
    {
        if (StaffProfile is not null)
            throw new UserAlreadyHasStaffProfileException();
        if (StudentProfile is not null)
            throw new UserAlreadyHasStudentProfileException();

        var profile = new StaffProfile(Id, departmentId, academicGroupId);
        profile.SetUser(this);
        StaffProfile = profile;
        return profile;
    }
}

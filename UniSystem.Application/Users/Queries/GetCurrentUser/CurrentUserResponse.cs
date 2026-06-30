namespace UniSystem.Application.Users.Queries.GetCurrentUser;

public record CurrentUserResponse(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    string? Patronymic,
    string FullName,
    string Sex,
    string Role,
    string? IconPath,
    StudentProfileResponse? StudentProfile,
    StaffProfileResponse? StaffProfile
);

public record StudentProfileResponse(
    string StudentTicket,
    int AcademicGroupId,
    string? AcademicGroupName,
    int StudentStatusId,
    string? StudentStatusName
);

public record StaffProfileResponse(
    int DepartmentId,
    string? DepartmentName,
    int? AcademicGroupId,
    string? AcademicGroupName
);

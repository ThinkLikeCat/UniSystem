using FluentValidation;
using UniSystem.Application.AcademicGroups;
using UniSystem.Application.Departments;
using UniSystem.Application.DocumentStatuses;
using UniSystem.Application.DocumentTypes;
using UniSystem.Application.Specialties;
using UniSystem.Application.StaffSubjects;
using UniSystem.Application.StudentStatuses;
using UniSystem.Application.Subjects;

namespace UniSystem.Application.Validators;

public class CreateDepartmentCommandValidator : AbstractValidator<CreateDepartmentCommand>
{
    public CreateDepartmentCommandValidator() => RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
}

public class UpdateDepartmentCommandValidator : AbstractValidator<UpdateDepartmentCommand>
{
    public UpdateDepartmentCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
    }
}

public class CreateSpecialtyCommandValidator : AbstractValidator<CreateSpecialtyCommand>
{
    public CreateSpecialtyCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Code).NotEmpty().MaximumLength(20);
        RuleFor(x => x.MaxDurationInYears).InclusiveBetween((short)1, (short)6);
    }
}

public class UpdateSpecialtyCommandValidator : AbstractValidator<UpdateSpecialtyCommand>
{
    public UpdateSpecialtyCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Code).NotEmpty().MaximumLength(20);
        RuleFor(x => x.MaxDurationInYears).InclusiveBetween((short)1, (short)6);
    }
}

public class CreateAcademicGroupCommandValidator : AbstractValidator<CreateAcademicGroupCommand>
{
    public CreateAcademicGroupCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(20);
        RuleFor(x => x.MaxCount).InclusiveBetween(1, 50);
        RuleFor(x => x.Course).InclusiveBetween((short)1, (short)6);
        RuleFor(x => x.SpecialtyId).GreaterThan(0);
    }
}

public class UpdateAcademicGroupCommandValidator : AbstractValidator<UpdateAcademicGroupCommand>
{
    public UpdateAcademicGroupCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(20);
        RuleFor(x => x.MaxCount).InclusiveBetween(1, 50);
        RuleFor(x => x.Course).InclusiveBetween((short)1, (short)6);
        RuleFor(x => x.SpecialtyId).GreaterThan(0);
    }
}

public class CreateStudentStatusCommandValidator : AbstractValidator<CreateStudentStatusCommand>
{
    public CreateStudentStatusCommandValidator() => RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
}

public class UpdateStudentStatusCommandValidator : AbstractValidator<UpdateStudentStatusCommand>
{
    public UpdateStudentStatusCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
    }
}

public class CreateDocumentStatusCommandValidator : AbstractValidator<CreateDocumentStatusCommand>
{
    public CreateDocumentStatusCommandValidator() => RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
}

public class UpdateDocumentStatusCommandValidator : AbstractValidator<UpdateDocumentStatusCommand>
{
    public UpdateDocumentStatusCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
    }
}

public class CreateDocumentTypeCommandValidator : AbstractValidator<CreateDocumentTypeCommand>
{
    public CreateDocumentTypeCommandValidator() => RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
}

public class UpdateDocumentTypeCommandValidator : AbstractValidator<UpdateDocumentTypeCommand>
{
    public UpdateDocumentTypeCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
    }
}

public class CreateSubjectCommandValidator : AbstractValidator<CreateSubjectCommand>
{
    public CreateSubjectCommandValidator() => RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
}

public class UpdateSubjectCommandValidator : AbstractValidator<UpdateSubjectCommand>
{
    public UpdateSubjectCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
    }
}

public class CreateStaffSubjectCommandValidator : AbstractValidator<CreateStaffSubjectCommand>
{
    public CreateStaffSubjectCommandValidator()
    {
        RuleFor(x => x.StaffId).NotEmpty();
        RuleFor(x => x.SubjectId).GreaterThan(0);
    }
}

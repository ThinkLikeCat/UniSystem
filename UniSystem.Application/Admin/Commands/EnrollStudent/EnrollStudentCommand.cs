using MediatR;
using Microsoft.EntityFrameworkCore;
using UniSystem.Application.Common.Interfaces;
using UniSystem.Domain.Exceptions;

namespace UniSystem.Application.Admin.Commands.EnrollStudent;

public record EnrollStudentCommand(Guid StudentId, int GroupId) : IRequest;

public class EnrollStudentCommandHandler : IRequestHandler<EnrollStudentCommand>
{
    private readonly IApplicationDbContext _context;

    public EnrollStudentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(EnrollStudentCommand request, CancellationToken cancellationToken)
    {
        var student = await _context.StudentProfiles
            .FirstOrDefaultAsync(s => s.Id == request.StudentId, cancellationToken);

        if (student is null)
            throw new DomainException("Student not found.");

        var group = await _context.AcademicGroups
            .Include(g => g.Students)
            .FirstOrDefaultAsync(g => g.Id == request.GroupId, cancellationToken);

        if (group is null)
            throw new DomainException("Academic group not found.");

        group.AddStudent(student);

        _context.Entry(student).Property("AcademicGroupId").CurrentValue = group.Id;

        await _context.SaveChangesAsync(cancellationToken);
    }
}

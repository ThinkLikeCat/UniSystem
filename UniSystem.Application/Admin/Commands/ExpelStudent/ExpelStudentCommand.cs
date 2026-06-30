using MediatR;
using Microsoft.EntityFrameworkCore;
using UniSystem.Application.Common.Interfaces;
using UniSystem.Domain.Exceptions;

namespace UniSystem.Application.Admin.Commands.ExpelStudent;

public record ExpelStudentCommand(Guid StudentId) : IRequest;

public class ExpelStudentCommandHandler : IRequestHandler<ExpelStudentCommand>
{
    private readonly IApplicationDbContext _context;

    public ExpelStudentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(ExpelStudentCommand request, CancellationToken cancellationToken)
    {
        var student = await _context.StudentProfiles
            .FirstOrDefaultAsync(s => s.Id == request.StudentId, cancellationToken);

        if (student is null)
            throw new DomainException("Student not found.");

        var expelledStatus = await _context.StudentStatuses
            .FirstOrDefaultAsync(s => s.Name == "Отчислен", cancellationToken);

        if (expelledStatus is null)
            throw new DomainException("Expelled status not found. Ensure 'Отчислен' status exists in the database.");

        _context.Entry(student).Property("StudentStatusId").CurrentValue = expelledStatus.Id;

        await _context.SaveChangesAsync(cancellationToken);
    }
}

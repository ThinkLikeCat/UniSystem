using MediatR;
using Microsoft.EntityFrameworkCore;
using UniSystem.Application.Common.Interfaces;
using UniSystem.Domain.Entities;
using UniSystem.Domain.Exceptions;
using UniSystem.Domain.ValueObjects.StudentStatus;

namespace UniSystem.Application.StudentStatuses;

public record CreateStudentStatusCommand(string Name) : IRequest<int>;

public class CreateStudentStatusCommandHandler : IRequestHandler<CreateStudentStatusCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateStudentStatusCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateStudentStatusCommand request, CancellationToken cancellationToken)
    {
        var entity = new StudentStatus(request.Name);
        _context.StudentStatuses.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}

public record UpdateStudentStatusCommand(int Id, string Name) : IRequest;

public class UpdateStudentStatusCommandHandler : IRequestHandler<UpdateStudentStatusCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateStudentStatusCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateStudentStatusCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.StudentStatuses
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (entity is null)
            throw new DomainException("Student status not found.");

        _context.Entry(entity).Property("Name").CurrentValue = new StatusName(request.Name);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

public record DeleteStudentStatusCommand(int Id) : IRequest;

public class DeleteStudentStatusCommandHandler : IRequestHandler<DeleteStudentStatusCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteStudentStatusCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteStudentStatusCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.StudentStatuses
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (entity is null)
            throw new DomainException("Student status not found.");

        _context.StudentStatuses.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

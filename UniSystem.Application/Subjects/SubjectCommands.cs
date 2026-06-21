using MediatR;
using Microsoft.EntityFrameworkCore;
using UniSystem.Application.Common.Interfaces;
using UniSystem.Domain.Entities;
using UniSystem.Domain.Exceptions;
using UniSystem.Domain.ValueObjects.Subject;

namespace UniSystem.Application.Subjects;

public record CreateSubjectCommand(string Name) : IRequest<int>;

public class CreateSubjectCommandHandler : IRequestHandler<CreateSubjectCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateSubjectCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateSubjectCommand request, CancellationToken cancellationToken)
    {
        var entity = new Subject(request.Name);
        _context.Subjects.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}

public record UpdateSubjectCommand(int Id, string Name) : IRequest;

public class UpdateSubjectCommandHandler : IRequestHandler<UpdateSubjectCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateSubjectCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateSubjectCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Subjects
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (entity is null)
            throw new DomainException("Subject not found.");

        _context.Entry(entity).Property("Name").CurrentValue = new SubjectName(request.Name);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

public record DeleteSubjectCommand(int Id) : IRequest;

public class DeleteSubjectCommandHandler : IRequestHandler<DeleteSubjectCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteSubjectCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteSubjectCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Subjects
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (entity is null)
            throw new DomainException("Subject not found.");

        _context.Subjects.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

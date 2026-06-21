using MediatR;
using Microsoft.EntityFrameworkCore;
using UniSystem.Application.Common.Interfaces;
using UniSystem.Domain.Entities;
using UniSystem.Domain.Exceptions;
using UniSystem.Domain.ValueObjects.DocumentStatus;

namespace UniSystem.Application.DocumentStatuses;

public record CreateDocumentStatusCommand(string Name) : IRequest<int>;

public class CreateDocumentStatusCommandHandler : IRequestHandler<CreateDocumentStatusCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateDocumentStatusCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateDocumentStatusCommand request, CancellationToken cancellationToken)
    {
        var entity = new DocumentStatus(request.Name);
        _context.DocumentStatuses.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}

public record UpdateDocumentStatusCommand(int Id, string Name) : IRequest;

public class UpdateDocumentStatusCommandHandler : IRequestHandler<UpdateDocumentStatusCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateDocumentStatusCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateDocumentStatusCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.DocumentStatuses
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (entity is null)
            throw new DomainException("Document status not found.");

        _context.Entry(entity).Property("Name").CurrentValue = new DocumentStatusName(request.Name);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

public record DeleteDocumentStatusCommand(int Id) : IRequest;

public class DeleteDocumentStatusCommandHandler : IRequestHandler<DeleteDocumentStatusCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteDocumentStatusCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteDocumentStatusCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.DocumentStatuses
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (entity is null)
            throw new DomainException("Document status not found.");

        _context.DocumentStatuses.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

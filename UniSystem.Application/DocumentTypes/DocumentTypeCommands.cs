using MediatR;
using Microsoft.EntityFrameworkCore;
using UniSystem.Application.Common.Interfaces;
using UniSystem.Domain.Entities;
using UniSystem.Domain.Exceptions;
using UniSystem.Domain.ValueObjects.DocumentType;

namespace UniSystem.Application.DocumentTypes;

public record CreateDocumentTypeCommand(string Name, bool RequiresAttachments, string TemplateText) : IRequest<int>;

public class CreateDocumentTypeCommandHandler : IRequestHandler<CreateDocumentTypeCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateDocumentTypeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateDocumentTypeCommand request, CancellationToken cancellationToken)
    {
        var entity = new DocumentType(request.Name, request.RequiresAttachments, request.TemplateText);
        _context.DocumentTypes.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}

public record UpdateDocumentTypeCommand(int Id, string Name, bool RequiresAttachments, string TemplateText) : IRequest;

public class UpdateDocumentTypeCommandHandler : IRequestHandler<UpdateDocumentTypeCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateDocumentTypeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateDocumentTypeCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.DocumentTypes
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (entity is null)
            throw new DomainException("Document type not found.");

        _context.Entry(entity).Property("Name").CurrentValue = new DocumentTypeName(request.Name);
        _context.Entry(entity).Property("RequiresAttachments").CurrentValue = request.RequiresAttachments;
        _context.Entry(entity).Property("TemplateText").CurrentValue = new TemplateText(request.TemplateText);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

public record DeleteDocumentTypeCommand(int Id) : IRequest;

public class DeleteDocumentTypeCommandHandler : IRequestHandler<DeleteDocumentTypeCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteDocumentTypeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteDocumentTypeCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.DocumentTypes
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (entity is null)
            throw new DomainException("Document type not found.");

        _context.DocumentTypes.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

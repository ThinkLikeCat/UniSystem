using MediatR;
using Microsoft.EntityFrameworkCore;
using UniSystem.Application.Common.Interfaces;
using UniSystem.Domain.Exceptions;

namespace UniSystem.Application.DocumentTypes;

public record DocumentTypeDto(int Id, string Name, bool RequiresAttachments, string TemplateText);

public record GetDocumentTypesQuery : IRequest<List<DocumentTypeDto>>;

public class GetDocumentTypesQueryHandler : IRequestHandler<GetDocumentTypesQuery, List<DocumentTypeDto>>
{
    private readonly IApplicationDbContext _context;

    public GetDocumentTypesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<DocumentTypeDto>> Handle(GetDocumentTypesQuery request, CancellationToken cancellationToken)
    {
        var entities = await _context.DocumentTypes
            .OrderBy(t => t.Name)
            .ToListAsync(cancellationToken);
        return entities.Select(t => new DocumentTypeDto(t.Id, t.Name.Value, t.RequiresAttachments, t.TemplateText.Value)).ToList();
    }
}

public record GetDocumentTypeByIdQuery(int Id) : IRequest<DocumentTypeDto>;

public class GetDocumentTypeByIdQueryHandler : IRequestHandler<GetDocumentTypeByIdQuery, DocumentTypeDto>
{
    private readonly IApplicationDbContext _context;

    public GetDocumentTypeByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DocumentTypeDto> Handle(GetDocumentTypeByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.DocumentTypes
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (entity is null)
            throw new DomainException("Document type not found.");

        return new DocumentTypeDto(entity.Id, entity.Name.Value, entity.RequiresAttachments, entity.TemplateText.Value);
    }
}

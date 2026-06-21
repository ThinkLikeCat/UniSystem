using MediatR;
using Microsoft.EntityFrameworkCore;
using UniSystem.Application.Common.Interfaces;
using UniSystem.Domain.Exceptions;

namespace UniSystem.Application.DocumentStatuses;

public record DocumentStatusDto(int Id, string Name);

public record GetDocumentStatusesQuery : IRequest<List<DocumentStatusDto>>;

public class GetDocumentStatusesQueryHandler : IRequestHandler<GetDocumentStatusesQuery, List<DocumentStatusDto>>
{
    private readonly IApplicationDbContext _context;

    public GetDocumentStatusesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<DocumentStatusDto>> Handle(GetDocumentStatusesQuery request, CancellationToken cancellationToken)
    {
        return await _context.DocumentStatuses
            .OrderBy(s => s.Name.Value)
            .Select(s => new DocumentStatusDto(s.Id, s.Name.Value))
            .ToListAsync(cancellationToken);
    }
}

public record GetDocumentStatusByIdQuery(int Id) : IRequest<DocumentStatusDto>;

public class GetDocumentStatusByIdQueryHandler : IRequestHandler<GetDocumentStatusByIdQuery, DocumentStatusDto>
{
    private readonly IApplicationDbContext _context;

    public GetDocumentStatusByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DocumentStatusDto> Handle(GetDocumentStatusByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.DocumentStatuses
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (entity is null)
            throw new DomainException("Document status not found.");

        return new DocumentStatusDto(entity.Id, entity.Name.Value);
    }
}

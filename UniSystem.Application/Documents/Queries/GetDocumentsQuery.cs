using MediatR;
using Microsoft.EntityFrameworkCore;
using UniSystem.Application.Common.Interfaces;
using UniSystem.Domain.Exceptions;

namespace UniSystem.Application.Documents.Queries;

public record DocumentListItemDto(
    Guid Id,
    string AuthorName,
    string DocumentTypeName,
    string CurrentStatusName,
    DateTimeOffset CreatedAt
);

public record GetDocumentsQuery(
    int? DocumentTypeId = null,
    int? StatusId = null,
    Guid? AuthorId = null
) : IRequest<List<DocumentListItemDto>>;

public class GetDocumentsQueryHandler : IRequestHandler<GetDocumentsQuery, List<DocumentListItemDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IUserContext _userContext;

    public GetDocumentsQueryHandler(IApplicationDbContext context, IUserContext userContext)
    {
        _context = context;
        _userContext = userContext;
    }

    public async Task<List<DocumentListItemDto>> Handle(GetDocumentsQuery request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId ?? throw new DomainException("User is not authenticated.");

        var query = _context.Documents
            .Include(d => d.Author)
            .Include(d => d.DocumentType)
            .Include(d => d.CurrentStatus)
            .AsQueryable();

        if (_userContext.Roles.Contains("StudentProfile"))
            query = query.Where(d => d.AuthorId == userId);

        if (request.DocumentTypeId is not null)
            query = query.Where(d => d.DocumentTypeId == request.DocumentTypeId);

        if (request.StatusId is not null)
            query = query.Where(d => d.DocumentCurrentStatusId == request.StatusId);

        if (request.AuthorId is not null)
            query = query.Where(d => d.AuthorId == request.AuthorId);

        return await query
            .OrderByDescending(d => d.CreatedAt)
            .Select(d => new DocumentListItemDto(
                d.Id.Value,
                d.Author.FullName,
                d.DocumentType.Name.Value,
                d.CurrentStatus.Name.Value,
                d.CreatedAt
            ))
            .ToListAsync(cancellationToken);
    }
}

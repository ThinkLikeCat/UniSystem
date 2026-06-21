using MediatR;
using Microsoft.EntityFrameworkCore;
using UniSystem.Application.Common.Interfaces;

namespace UniSystem.Application.Admin.Queries.Statistics;

public record TypeStatDto(
    int DocumentTypeId,
    string DocumentTypeName,
    int Count
);

public record GetStatisticsByTypeQuery : IRequest<List<TypeStatDto>>;

public class GetStatisticsByTypeQueryHandler : IRequestHandler<GetStatisticsByTypeQuery, List<TypeStatDto>>
{
    private readonly IApplicationDbContext _context;

    public GetStatisticsByTypeQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<TypeStatDto>> Handle(GetStatisticsByTypeQuery request, CancellationToken cancellationToken)
    {
        var stats = await _context.Documents
            .GroupBy(d => new { d.DocumentTypeId, d.DocumentType.Name.Value })
            .Select(g => new TypeStatDto(g.Key.DocumentTypeId, g.Key.Value, g.Count()))
            .OrderByDescending(d => d.Count)
            .ToListAsync(cancellationToken);

        return stats;
    }
}

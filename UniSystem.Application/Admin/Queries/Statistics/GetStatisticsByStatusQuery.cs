using MediatR;
using Microsoft.EntityFrameworkCore;
using UniSystem.Application.Common.Interfaces;

namespace UniSystem.Application.Admin.Queries.Statistics;

public record StatusStatDto(
    int StatusId,
    string StatusName,
    int Count
);

public record GetStatisticsByStatusQuery : IRequest<List<StatusStatDto>>;

public class GetStatisticsByStatusQueryHandler : IRequestHandler<GetStatisticsByStatusQuery, List<StatusStatDto>>
{
    private readonly IApplicationDbContext _context;

    public GetStatisticsByStatusQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<StatusStatDto>> Handle(GetStatisticsByStatusQuery request, CancellationToken cancellationToken)
    {
        var stats = await _context.Documents
            .GroupBy(d => new { d.DocumentCurrentStatusId, d.CurrentStatus.Name.Value })
            .Select(g => new StatusStatDto(g.Key.DocumentCurrentStatusId, g.Key.Value, g.Count()))
            .OrderBy(d => d.StatusId)
            .ToListAsync(cancellationToken);

        return stats;
    }
}

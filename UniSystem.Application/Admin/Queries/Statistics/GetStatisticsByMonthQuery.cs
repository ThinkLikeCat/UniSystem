using MediatR;
using Microsoft.EntityFrameworkCore;
using UniSystem.Application.Common.Interfaces;

namespace UniSystem.Application.Admin.Queries.Statistics;

public record MonthlyStatDto(
    int Year,
    int Month,
    int Count
);

public record GetStatisticsByMonthQuery : IRequest<List<MonthlyStatDto>>;

public class GetStatisticsByMonthQueryHandler : IRequestHandler<GetStatisticsByMonthQuery, List<MonthlyStatDto>>
{
    private readonly IApplicationDbContext _context;

    public GetStatisticsByMonthQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<MonthlyStatDto>> Handle(GetStatisticsByMonthQuery request, CancellationToken cancellationToken)
    {
        var documents = await _context.Documents
            .GroupBy(d => new { d.CreatedAt.Year, d.CreatedAt.Month })
            .Select(g => new MonthlyStatDto(g.Key.Year, g.Key.Month, g.Count()))
            .OrderBy(d => d.Year).ThenBy(d => d.Month)
            .ToListAsync(cancellationToken);

        return documents;
    }
}

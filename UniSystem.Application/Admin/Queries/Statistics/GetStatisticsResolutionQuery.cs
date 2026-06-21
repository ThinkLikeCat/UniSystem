using MediatR;
using Microsoft.EntityFrameworkCore;
using UniSystem.Application.Common.Interfaces;

namespace UniSystem.Application.Admin.Queries.Statistics;

public record ResolutionStatDto(
    int WithResolution,
    int WithoutResolution,
    int Total
);

public record GetStatisticsResolutionQuery : IRequest<ResolutionStatDto>;

public class GetStatisticsResolutionQueryHandler : IRequestHandler<GetStatisticsResolutionQuery, ResolutionStatDto>
{
    private readonly IApplicationDbContext _context;

    public GetStatisticsResolutionQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ResolutionStatDto> Handle(GetStatisticsResolutionQuery request, CancellationToken cancellationToken)
    {
        var withResolution = await _context.Documents
            .CountAsync(d => d.ResolutionComment != null && d.ResolutionComment != "", cancellationToken);

        var total = await _context.Documents.CountAsync(cancellationToken);

        return new ResolutionStatDto(withResolution, total - withResolution, total);
    }
}

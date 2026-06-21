using MediatR;
using Microsoft.EntityFrameworkCore;
using UniSystem.Application.Common.Interfaces;

namespace UniSystem.Application.Admin.Queries.Statistics;

public record StatisticsSummaryDto(
    int TotalDocuments,
    int Draft,
    int UnderSecretaryReview,
    int Rework,
    int UnderDeanReview,
    int Approved,
    int Rejected
);

public record GetStatisticsSummaryQuery : IRequest<StatisticsSummaryDto>;

public class GetStatisticsSummaryQueryHandler : IRequestHandler<GetStatisticsSummaryQuery, StatisticsSummaryDto>
{
    private readonly IApplicationDbContext _context;

    public GetStatisticsSummaryQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StatisticsSummaryDto> Handle(GetStatisticsSummaryQuery request, CancellationToken cancellationToken)
    {
        var statuses = await _context.DocumentStatuses.ToListAsync(cancellationToken);

        var draftId = statuses.First(s => s.Name.Value == "Черновик").Id;
        var secretaryId = statuses.First(s => s.Name.Value == "На проверке секретаря").Id;
        var reworkId = statuses.First(s => s.Name.Value == "На доработку").Id;
        var deanId = statuses.First(s => s.Name.Value == "На проверке декана").Id;
        var approvedId = statuses.First(s => s.Name.Value == "Утверждён").Id;
        var rejectedId = statuses.First(s => s.Name.Value == "Отклонён").Id;

        var allDocuments = await _context.Documents.ToListAsync(cancellationToken);

        return new StatisticsSummaryDto(
            allDocuments.Count,
            allDocuments.Count(d => d.DocumentCurrentStatusId == draftId),
            allDocuments.Count(d => d.DocumentCurrentStatusId == secretaryId),
            allDocuments.Count(d => d.DocumentCurrentStatusId == reworkId),
            allDocuments.Count(d => d.DocumentCurrentStatusId == deanId),
            allDocuments.Count(d => d.DocumentCurrentStatusId == approvedId),
            allDocuments.Count(d => d.DocumentCurrentStatusId == rejectedId)
        );
    }
}

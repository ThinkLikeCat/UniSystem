using MediatR;
using Microsoft.EntityFrameworkCore;
using UniSystem.Application.Common.Interfaces;
using UniSystem.Domain.Exceptions;

namespace UniSystem.Application.StaffSubjects;

public record StaffSubjectDto(Guid StaffId, int SubjectId, string SubjectName);

public record GetStaffSubjectsQuery(Guid StaffId) : IRequest<List<StaffSubjectDto>>;

public class GetStaffSubjectsQueryHandler : IRequestHandler<GetStaffSubjectsQuery, List<StaffSubjectDto>>
{
    private readonly IApplicationDbContext _context;

    public GetStaffSubjectsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<StaffSubjectDto>> Handle(GetStaffSubjectsQuery request, CancellationToken cancellationToken)
    {
        var staffExists = await _context.StaffProfiles
            .AnyAsync(p => p.Id == request.StaffId, cancellationToken);

        if (!staffExists)
            throw new DomainException("Staff profile not found.");

        var entities = await _context.StaffSubjects
            .Where(s => s.StaffId == request.StaffId)
            .Include(s => s.Subject)
            .OrderBy(s => s.Subject.Name)
            .ToListAsync(cancellationToken);
        return entities.Select(s => new StaffSubjectDto(s.StaffId, s.SubjectId, s.Subject.Name.Value)).ToList();
    }
}

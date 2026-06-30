using MediatR;
using Microsoft.EntityFrameworkCore;
using UniSystem.Application.Common.Interfaces;
using UniSystem.Domain.Exceptions;

namespace UniSystem.Application.AcademicGroups;

public record AcademicGroupDto(int Id, string Name, int MaxCount, short Course, int SpecialtyId, string SpecialtyName);

public record GetAcademicGroupsQuery : IRequest<List<AcademicGroupDto>>;

public class GetAcademicGroupsQueryHandler : IRequestHandler<GetAcademicGroupsQuery, List<AcademicGroupDto>>
{
    private readonly IApplicationDbContext _context;

    public GetAcademicGroupsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<AcademicGroupDto>> Handle(GetAcademicGroupsQuery request, CancellationToken cancellationToken)
    {
        var entities = await _context.AcademicGroups
            .Include(g => g.Specialty)
            .OrderBy(g => g.Name)
            .ToListAsync(cancellationToken);
        return entities.Select(g => new AcademicGroupDto(
            g.Id,
            g.Name.Value,
            g.MaxCount.Value,
            (short)g.Course.Value,
            g.SpecialtyId,
            g.Specialty.Name.Value)).ToList();
    }
}

public record GetAcademicGroupByIdQuery(int Id) : IRequest<AcademicGroupDto>;

public class GetAcademicGroupByIdQueryHandler : IRequestHandler<GetAcademicGroupByIdQuery, AcademicGroupDto>
{
    private readonly IApplicationDbContext _context;

    public GetAcademicGroupByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AcademicGroupDto> Handle(GetAcademicGroupByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.AcademicGroups
            .Include(g => g.Specialty)
            .FirstOrDefaultAsync(g => g.Id == request.Id, cancellationToken);

        if (entity is null)
            throw new DomainException("Academic group not found.");

        return new AcademicGroupDto(
            entity.Id,
            entity.Name.Value,
            entity.MaxCount.Value,
            (short)entity.Course.Value,
            entity.SpecialtyId,
            entity.Specialty.Name.Value);
    }
}

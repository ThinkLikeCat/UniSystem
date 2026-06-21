using MediatR;
using Microsoft.EntityFrameworkCore;
using UniSystem.Application.Common.Interfaces;
using UniSystem.Domain.Exceptions;

namespace UniSystem.Application.Specialties;

public record SpecialtyDto(int Id, string Name, string Code, short MaxDurationInYears);

public record GetSpecialtiesQuery : IRequest<List<SpecialtyDto>>;

public class GetSpecialtiesQueryHandler : IRequestHandler<GetSpecialtiesQuery, List<SpecialtyDto>>
{
    private readonly IApplicationDbContext _context;

    public GetSpecialtiesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<SpecialtyDto>> Handle(GetSpecialtiesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Specialties
            .OrderBy(s => s.Name.Value)
            .Select(s => new SpecialtyDto(s.Id, s.Name.Value, s.Code.Value, s.MaxDurationInYears.Value))
            .ToListAsync(cancellationToken);
    }
}

public record GetSpecialtyByIdQuery(int Id) : IRequest<SpecialtyDto>;

public class GetSpecialtyByIdQueryHandler : IRequestHandler<GetSpecialtyByIdQuery, SpecialtyDto>
{
    private readonly IApplicationDbContext _context;

    public GetSpecialtyByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SpecialtyDto> Handle(GetSpecialtyByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Specialties
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (entity is null)
            throw new DomainException("Specialty not found.");

        return new SpecialtyDto(entity.Id, entity.Name.Value, entity.Code.Value, entity.MaxDurationInYears.Value);
    }
}

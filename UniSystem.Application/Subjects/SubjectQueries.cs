using MediatR;
using Microsoft.EntityFrameworkCore;
using UniSystem.Application.Common.Interfaces;
using UniSystem.Domain.Exceptions;

namespace UniSystem.Application.Subjects;

public record SubjectDto(int Id, string Name);

public record GetSubjectsQuery : IRequest<List<SubjectDto>>;

public class GetSubjectsQueryHandler : IRequestHandler<GetSubjectsQuery, List<SubjectDto>>
{
    private readonly IApplicationDbContext _context;

    public GetSubjectsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<SubjectDto>> Handle(GetSubjectsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Subjects
            .OrderBy(s => s.Name.Value)
            .Select(s => new SubjectDto(s.Id, s.Name.Value))
            .ToListAsync(cancellationToken);
    }
}

public record GetSubjectByIdQuery(int Id) : IRequest<SubjectDto>;

public class GetSubjectByIdQueryHandler : IRequestHandler<GetSubjectByIdQuery, SubjectDto>
{
    private readonly IApplicationDbContext _context;

    public GetSubjectByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SubjectDto> Handle(GetSubjectByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Subjects
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (entity is null)
            throw new DomainException("Subject not found.");

        return new SubjectDto(entity.Id, entity.Name.Value);
    }
}

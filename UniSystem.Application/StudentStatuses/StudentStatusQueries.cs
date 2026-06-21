using MediatR;
using Microsoft.EntityFrameworkCore;
using UniSystem.Application.Common.Interfaces;
using UniSystem.Domain.Exceptions;

namespace UniSystem.Application.StudentStatuses;

public record StudentStatusDto(int Id, string Name);

public record GetStudentStatusesQuery : IRequest<List<StudentStatusDto>>;

public class GetStudentStatusesQueryHandler : IRequestHandler<GetStudentStatusesQuery, List<StudentStatusDto>>
{
    private readonly IApplicationDbContext _context;

    public GetStudentStatusesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<StudentStatusDto>> Handle(GetStudentStatusesQuery request, CancellationToken cancellationToken)
    {
        return await _context.StudentStatuses
            .OrderBy(s => s.Name.Value)
            .Select(s => new StudentStatusDto(s.Id, s.Name.Value))
            .ToListAsync(cancellationToken);
    }
}

public record GetStudentStatusByIdQuery(int Id) : IRequest<StudentStatusDto>;

public class GetStudentStatusByIdQueryHandler : IRequestHandler<GetStudentStatusByIdQuery, StudentStatusDto>
{
    private readonly IApplicationDbContext _context;

    public GetStudentStatusByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StudentStatusDto> Handle(GetStudentStatusByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.StudentStatuses
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (entity is null)
            throw new DomainException("Student status not found.");

        return new StudentStatusDto(entity.Id, entity.Name.Value);
    }
}

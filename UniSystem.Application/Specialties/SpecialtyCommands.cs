using MediatR;
using Microsoft.EntityFrameworkCore;
using UniSystem.Application.Common.Interfaces;
using UniSystem.Domain.Entities;
using UniSystem.Domain.Exceptions;
using UniSystem.Domain.ValueObjects.Specialty;

namespace UniSystem.Application.Specialties;

public record CreateSpecialtyCommand(string Name, string Code, short MaxDurationInYears) : IRequest<int>;

public class CreateSpecialtyCommandHandler : IRequestHandler<CreateSpecialtyCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateSpecialtyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateSpecialtyCommand request, CancellationToken cancellationToken)
    {
        var entity = new Specialty(request.Name, request.Code, request.MaxDurationInYears);
        _context.Specialties.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}

public record UpdateSpecialtyCommand(int Id, string Name, string Code, short MaxDurationInYears) : IRequest;

public class UpdateSpecialtyCommandHandler : IRequestHandler<UpdateSpecialtyCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateSpecialtyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateSpecialtyCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Specialties
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (entity is null)
            throw new DomainException("Specialty not found.");

        _context.Entry(entity).Property("Name").CurrentValue = new SpecialtyName(request.Name);
        _context.Entry(entity).Property("Code").CurrentValue = new SpecialtyCode(request.Code);
        _context.Entry(entity).Property("MaxDurationInYears").CurrentValue = new SpecialtyDuration(request.MaxDurationInYears);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

public record DeleteSpecialtyCommand(int Id) : IRequest;

public class DeleteSpecialtyCommandHandler : IRequestHandler<DeleteSpecialtyCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteSpecialtyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteSpecialtyCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Specialties
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (entity is null)
            throw new DomainException("Specialty not found.");

        _context.Specialties.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

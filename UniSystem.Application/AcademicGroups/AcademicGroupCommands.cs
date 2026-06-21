using MediatR;
using Microsoft.EntityFrameworkCore;
using UniSystem.Application.Common.Interfaces;
using UniSystem.Domain.Entities;
using UniSystem.Domain.Exceptions;
using UniSystem.Domain.ValueObjects.AcademicGroup;

namespace UniSystem.Application.AcademicGroups;

public record CreateAcademicGroupCommand(string Name, int MaxCount, short Course, int SpecialtyId) : IRequest<int>;

public class CreateAcademicGroupCommandHandler : IRequestHandler<CreateAcademicGroupCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateAcademicGroupCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateAcademicGroupCommand request, CancellationToken cancellationToken)
    {
        var entity = new AcademicGroup(request.Name, request.MaxCount, request.Course, request.SpecialtyId);
        _context.AcademicGroups.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}

public record UpdateAcademicGroupCommand(int Id, string Name, int MaxCount, short Course, int SpecialtyId) : IRequest;

public class UpdateAcademicGroupCommandHandler : IRequestHandler<UpdateAcademicGroupCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateAcademicGroupCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateAcademicGroupCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.AcademicGroups
            .FirstOrDefaultAsync(g => g.Id == request.Id, cancellationToken);

        if (entity is null)
            throw new DomainException("Academic group not found.");

        _context.Entry(entity).Property("Name").CurrentValue = new GroupName(request.Name);
        _context.Entry(entity).Property("MaxCount").CurrentValue = new GroupMaxCount(request.MaxCount);
        _context.Entry(entity).Property("Course").CurrentValue = new Course(request.Course);
        _context.Entry(entity).Property("SpecialtyId").CurrentValue = request.SpecialtyId;
        await _context.SaveChangesAsync(cancellationToken);
    }
}

public record DeleteAcademicGroupCommand(int Id) : IRequest;

public class DeleteAcademicGroupCommandHandler : IRequestHandler<DeleteAcademicGroupCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteAcademicGroupCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteAcademicGroupCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.AcademicGroups
            .FirstOrDefaultAsync(g => g.Id == request.Id, cancellationToken);

        if (entity is null)
            throw new DomainException("Academic group not found.");

        _context.AcademicGroups.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

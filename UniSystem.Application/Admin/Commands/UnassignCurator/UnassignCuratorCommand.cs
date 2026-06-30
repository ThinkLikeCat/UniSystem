using MediatR;
using Microsoft.EntityFrameworkCore;
using UniSystem.Application.Common.Interfaces;
using UniSystem.Domain.Exceptions;

namespace UniSystem.Application.Admin.Commands.UnassignCurator;

public record UnassignCuratorCommand(int GroupId) : IRequest;

public class UnassignCuratorCommandHandler : IRequestHandler<UnassignCuratorCommand>
{
    private readonly IApplicationDbContext _context;

    public UnassignCuratorCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UnassignCuratorCommand request, CancellationToken cancellationToken)
    {
        var curator = await _context.StaffProfiles
            .FirstOrDefaultAsync(s => s.AcademicGroupId == request.GroupId, cancellationToken);

        if (curator is null)
            throw new DomainException("No curator assigned to this group.");

        _context.Entry(curator).Property("AcademicGroupId").CurrentValue = null;

        await _context.SaveChangesAsync(cancellationToken);
    }
}

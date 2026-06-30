using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UniSystem.Application.Common.Interfaces;
using UniSystem.Domain.Entities;
using UniSystem.Domain.Enums;
using UniSystem.Domain.Exceptions;

namespace UniSystem.Application.Admin.Commands.AssignCurator;

public record AssignCuratorCommand(Guid StaffId, int GroupId) : IRequest;

public class AssignCuratorCommandHandler : IRequestHandler<AssignCuratorCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<User> _userManager;

    public AssignCuratorCommandHandler(IApplicationDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task Handle(AssignCuratorCommand request, CancellationToken cancellationToken)
    {
        var group = await _context.AcademicGroups
            .FirstOrDefaultAsync(g => g.Id == request.GroupId, cancellationToken);

        if (group is null)
            throw new DomainException("Academic group not found.");

        var staff = await _context.StaffProfiles
            .FirstOrDefaultAsync(s => s.Id == request.StaffId, cancellationToken);

        if (staff is null)
            throw new DomainException("Staff profile not found.");

        var user = await _userManager.FindByIdAsync(request.StaffId.ToString());
        if (user is null)
            throw new DomainException("User not found.");

        var isCurator = await _userManager.IsInRoleAsync(user, SystemRoleName.Curator.ToString());
        if (!isCurator)
            throw new DomainException("User does not have the Curator role.");

        var existingCurator = await _context.StaffProfiles
            .FirstOrDefaultAsync(s => s.AcademicGroupId == request.GroupId && s.Id != request.StaffId, cancellationToken);

        if (existingCurator is not null)
            _context.Entry(existingCurator).Property("AcademicGroupId").CurrentValue = null;

        _context.Entry(staff).Property("AcademicGroupId").CurrentValue = request.GroupId;

        await _context.SaveChangesAsync(cancellationToken);
    }
}

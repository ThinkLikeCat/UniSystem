using MediatR;
using Microsoft.EntityFrameworkCore;
using UniSystem.Application.Common.Interfaces;
using UniSystem.Domain.Exceptions;

namespace UniSystem.Application.Users.Commands.UpdateStaffProfile;

public record UpdateStaffProfileCommand(
    int? DepartmentId = null,
    int? AcademicGroupId = null
) : IRequest;

public class UpdateStaffProfileCommandHandler : IRequestHandler<UpdateStaffProfileCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IUserContext _userContext;

    public UpdateStaffProfileCommandHandler(IApplicationDbContext context, IUserContext userContext)
    {
        _context = context;
        _userContext = userContext;
    }

    public async Task Handle(UpdateStaffProfileCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId ?? throw new DomainException("User is not authenticated.");

        var profile = await _context.StaffProfiles
            .FirstOrDefaultAsync(p => p.Id == userId, cancellationToken);

        if (profile is null)
            throw new DomainException("Staff profile not found.");

        if (request.DepartmentId is not null)
            _context.Entry(profile).Property("DepartmentId").CurrentValue = request.DepartmentId.Value;

        if (request.AcademicGroupId.HasValue)
            _context.Entry(profile).Property("AcademicGroupId").CurrentValue = request.AcademicGroupId.Value;

        await _context.SaveChangesAsync(cancellationToken);
    }
}

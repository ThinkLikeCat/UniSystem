using MediatR;
using Microsoft.EntityFrameworkCore;
using UniSystem.Application.Common.Interfaces;
using UniSystem.Domain.Exceptions;

namespace UniSystem.Application.Users.Commands.UpdateStudentProfile;

public record UpdateStudentProfileCommand(
    string? StudentTicket = null
) : IRequest;

public class UpdateStudentProfileCommandHandler : IRequestHandler<UpdateStudentProfileCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IUserContext _userContext;

    public UpdateStudentProfileCommandHandler(IApplicationDbContext context, IUserContext userContext)
    {
        _context = context;
        _userContext = userContext;
    }

    public async Task Handle(UpdateStudentProfileCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId ?? throw new DomainException("User is not authenticated.");

        var profile = await _context.StudentProfiles
            .FirstOrDefaultAsync(p => p.Id == userId, cancellationToken);

        if (profile is null)
            throw new DomainException("Student profile not found.");

        if (request.StudentTicket is not null)
            _context.Entry(profile).Property("StudentTicket").CurrentValue = request.StudentTicket;

        await _context.SaveChangesAsync(cancellationToken);
    }
}

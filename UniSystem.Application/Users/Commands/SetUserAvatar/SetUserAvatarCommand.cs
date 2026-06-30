using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UniSystem.Application.Common.Interfaces;
using UniSystem.Domain.Entities;
using UniSystem.Domain.Exceptions;
using UniSystem.Domain.ValueObjects.User;

namespace UniSystem.Application.Users.Commands.SetUserAvatar;

public record SetUserAvatarCommand(string FileName, Stream Content) : IRequest<string>;

public class SetUserAvatarCommandHandler : IRequestHandler<SetUserAvatarCommand, string>
{
    private readonly UserManager<User> _userManager;
    private readonly IApplicationDbContext _context;
    private readonly IFileStorageService _fileStorage;
    private readonly IUserContext _userContext;

    public SetUserAvatarCommandHandler(
        UserManager<User> userManager,
        IApplicationDbContext context,
        IFileStorageService fileStorage,
        IUserContext userContext)
    {
        _userManager = userManager;
        _context = context;
        _fileStorage = fileStorage;
        _userContext = userContext;
    }

    public async Task<string> Handle(SetUserAvatarCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId ?? throw new DomainException("User is not authenticated.");

        var user = await _userManager.Users
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user is null)
            throw new DomainException("User not found.");

        var filePath = await _fileStorage.SaveAsync(userId, request.FileName, request.Content, cancellationToken);

        _context.Entry(user).Property("IconPath").CurrentValue = new IconPath(filePath);

        await _context.SaveChangesAsync(cancellationToken);

        return filePath;
    }
}

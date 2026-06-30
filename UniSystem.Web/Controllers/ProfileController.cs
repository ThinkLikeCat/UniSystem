using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniSystem.Application.Common.Interfaces;
using UniSystem.Application.Users.Commands.ChangePassword;
using UniSystem.Application.Users.Commands.SetUserAvatar;
using UniSystem.Application.Users.Commands.UpdateStaffProfile;
using UniSystem.Application.Users.Commands.UpdateStudentProfile;
using UniSystem.Application.Users.Queries.GetCurrentUser;
using UniSystem.Domain.Entities;
using UniSystem.Domain.Exceptions;

namespace UniSystem.Web.Controllers;

[ApiController]
[Route("api/profile")]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProfileController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<CurrentUserResponse>> Get()
    {
        return Ok(await _mediator.Send(new GetCurrentUserQuery()));
    }

    [HttpPut("student")]
    [Authorize(Roles = "StudentProfile")]
    public async Task<IActionResult> UpdateStudent([FromBody] UpdateStudentProfileCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPut("staff")]
    [Authorize(Roles = "StaffProfile,Dean,Secretary,Curator")]
    public async Task<IActionResult> UpdateStaff([FromBody] UpdateStaffProfileCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPost("avatar")]
    [RequestSizeLimit(5 * 1024 * 1024)]
    public async Task<ActionResult<string>> UploadAvatar(IFormFile file)
    {
        if (file is null || file.Length == 0)
            return BadRequest("File not selected or empty.");

        if (file.Length > 5 * 1024 * 1024)
            return BadRequest("File size must not exceed 5 MB.");

        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (ext != ".jpg" && ext != ".jpeg" && ext != ".png" && ext != ".gif" && ext != ".webp")
            return BadRequest("Only image files are allowed (jpg, jpeg, png, gif, webp).");

        await using var stream = file.OpenReadStream();
        var filePath = await _mediator.Send(new SetUserAvatarCommand(file.FileName, stream));
        return Ok(new { iconPath = filePath });
    }

    [HttpGet("avatar")]
    public async Task<IActionResult> GetAvatar(
        [FromServices] IFileStorageService fileStorage,
        [FromServices] UserManager<User> userManager,
        [FromServices] IUserContext userContext)
    {
        var userId = userContext.UserId;
        if (userId is null)
            return NotFound();

        var user = await userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user is null || user.IconPath is null)
            return NotFound();

        var stream = await fileStorage.GetAsync(user.IconPath.Value);
        if (stream is null)
            return NotFound();

        var ext = Path.GetExtension(user.IconPath.Value).ToLowerInvariant();
        var contentType = ext switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".webp" => "image/webp",
            _ => "application/octet-stream"
        };

        return File(stream, contentType);
    }
}

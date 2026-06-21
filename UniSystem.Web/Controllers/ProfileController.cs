using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniSystem.Application.Users.Commands.ChangePassword;
using UniSystem.Application.Users.Commands.UpdateStaffProfile;
using UniSystem.Application.Users.Commands.UpdateStudentProfile;
using UniSystem.Application.Users.Queries.GetCurrentUser;

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
}

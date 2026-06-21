using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniSystem.Application.Users.Commands.Login;
using UniSystem.Application.Users.Commands.RegisterUser;
using UniSystem.Application.Users.Queries.GetCurrentUser;

namespace UniSystem.Web.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("register")]
    public async Task<ActionResult<Guid>> Register([FromBody] RegisterUserCommand command)
    {
        var userId = await _mediator.Send(command);
        return Ok(userId);
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login([FromBody] LoginCommand command)
    {
        var token = await _mediator.Send(command);
        return Ok(token);
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<CurrentUserResponse>> Me()
    {
        var query = new GetCurrentUserQuery();
        var response = await _mediator.Send(query);
        return Ok(response);
    }
}

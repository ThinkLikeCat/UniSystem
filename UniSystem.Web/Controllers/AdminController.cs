using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniSystem.Application.Admin.Commands.AssignCurator;
using UniSystem.Application.Admin.Commands.CreateUser;
using UniSystem.Application.Admin.Commands.DeleteUser;
using UniSystem.Application.Admin.Commands.EnrollStudent;
using UniSystem.Application.Admin.Commands.ExpelStudent;
using UniSystem.Application.Admin.Commands.ResetPassword;
using UniSystem.Application.Admin.Commands.UnassignCurator;
using UniSystem.Application.Admin.Commands.UpdateUser;
using UniSystem.Application.Admin.Queries.GetUserById;
using UniSystem.Application.Admin.Queries.GetUsers;
using UniSystem.Application.Admin.Queries.Statistics;

namespace UniSystem.Web.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("users")]
    public async Task<ActionResult<List<UserListItemDto>>> GetUsers([FromQuery] string? roleFilter)
    {
        var query = new GetUsersQuery(roleFilter);
        var users = await _mediator.Send(query);
        return Ok(users);
    }

    [HttpGet("users/{id:guid}")]
    public async Task<ActionResult<UserDetailDto>> GetUserById(Guid id)
    {
        var query = new GetUserByIdQuery(id);
        var user = await _mediator.Send(query);
        return Ok(user);
    }

    [HttpPost("users")]
    public async Task<ActionResult<Guid>> CreateUser([FromBody] CreateUserCommand command)
    {
        var userId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetUserById), new { id = userId }, userId);
    }

    [HttpPut("users/{id:guid}")]
    public async Task<ActionResult> UpdateUser(Guid id, [FromBody] UpdateUserCommand command)
    {
        command = command with { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("users/{id:guid}")]
    public async Task<ActionResult> DeleteUser(Guid id)
    {
        await _mediator.Send(new DeleteUserCommand(id));
        return NoContent();
    }

    [HttpPost("users/{id:guid}/reset-password")]
    public async Task<ActionResult> ResetPassword(Guid id, [FromBody] ResetPasswordCommand command)
    {
        command = command with { UserId = id };
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpGet("statistics/summary")]
    public async Task<ActionResult<StatisticsSummaryDto>> GetSummary()
    {
        return Ok(await _mediator.Send(new GetStatisticsSummaryQuery()));
    }

    [HttpGet("statistics/by-month")]
    public async Task<ActionResult<List<MonthlyStatDto>>> GetByMonth()
    {
        return Ok(await _mediator.Send(new GetStatisticsByMonthQuery()));
    }

    [HttpGet("statistics/by-type")]
    public async Task<ActionResult<List<TypeStatDto>>> GetByType()
    {
        return Ok(await _mediator.Send(new GetStatisticsByTypeQuery()));
    }

    [HttpGet("statistics/resolution")]
    public async Task<ActionResult<ResolutionStatDto>> GetResolution()
    {
        return Ok(await _mediator.Send(new GetStatisticsResolutionQuery()));
    }

    [HttpGet("statistics/by-status")]
    public async Task<ActionResult<List<StatusStatDto>>> GetByStatus()
    {
        return Ok(await _mediator.Send(new GetStatisticsByStatusQuery()));
    }

    [HttpPost("students/{studentId}/enroll")]
    public async Task<ActionResult> EnrollStudent(Guid studentId, [FromBody] EnrollStudentCommand command)
    {
        command = command with { StudentId = studentId };
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPost("students/{studentId}/expel")]
    public async Task<ActionResult> ExpelStudent(Guid studentId)
    {
        await _mediator.Send(new ExpelStudentCommand(studentId));
        return NoContent();
    }

    [HttpPost("groups/{groupId:int}/curator")]
    public async Task<ActionResult> AssignCurator(int groupId, [FromBody] AssignCuratorCommand command)
    {
        command = command with { GroupId = groupId };
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("groups/{groupId:int}/curator")]
    public async Task<ActionResult> UnassignCurator(int groupId)
    {
        await _mediator.Send(new UnassignCuratorCommand(groupId));
        return NoContent();
    }
}

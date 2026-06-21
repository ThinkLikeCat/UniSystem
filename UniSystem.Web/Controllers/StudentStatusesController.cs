using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniSystem.Application.StudentStatuses;

namespace UniSystem.Web.Controllers;

[ApiController]
[Authorize]
[Route("api/student-statuses")]
public class StudentStatusesController : ControllerBase
{
    private readonly IMediator _mediator;

    public StudentStatusesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<StudentStatusDto>>> GetAll()
    {
        return Ok(await _mediator.Send(new GetStudentStatusesQuery()));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<StudentStatusDto>> GetById(int id)
    {
        return Ok(await _mediator.Send(new GetStudentStatusByIdQuery(id)));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<int>> Create([FromBody] CreateStudentStatusCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateStudentStatusCommand command)
    {
        if (command.Id != id)
            return BadRequest("ID in URL and request body do not match.");

        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteStudentStatusCommand(id));
        return NoContent();
    }
}

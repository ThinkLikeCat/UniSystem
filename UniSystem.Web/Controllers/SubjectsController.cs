using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniSystem.Application.Subjects;

namespace UniSystem.Web.Controllers;

[ApiController]
[Authorize]
[Route("api/subjects")]
public class SubjectsController : ControllerBase
{
    private readonly IMediator _mediator;

    public SubjectsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<SubjectDto>>> GetAll()
    {
        return Ok(await _mediator.Send(new GetSubjectsQuery()));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SubjectDto>> GetById(int id)
    {
        return Ok(await _mediator.Send(new GetSubjectByIdQuery(id)));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<int>> Create([FromBody] CreateSubjectCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateSubjectCommand command)
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
        await _mediator.Send(new DeleteSubjectCommand(id));
        return NoContent();
    }
}

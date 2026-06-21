using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniSystem.Application.AcademicGroups;

namespace UniSystem.Web.Controllers;

[ApiController]
[Authorize]
[Route("api/academic-groups")]
public class AcademicGroupsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AcademicGroupsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<AcademicGroupDto>>> GetAll()
    {
        return Ok(await _mediator.Send(new GetAcademicGroupsQuery()));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AcademicGroupDto>> GetById(int id)
    {
        return Ok(await _mediator.Send(new GetAcademicGroupByIdQuery(id)));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<int>> Create([FromBody] CreateAcademicGroupCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAcademicGroupCommand command)
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
        await _mediator.Send(new DeleteAcademicGroupCommand(id));
        return NoContent();
    }
}

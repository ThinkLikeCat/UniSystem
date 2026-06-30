using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniSystem.Application.Specialties;

namespace UniSystem.Web.Controllers;

[ApiController]
[Authorize]
[Route("api/specialties")]
public class SpecialtiesController : ControllerBase
{
    private readonly IMediator _mediator;

    public SpecialtiesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<SpecialtyDto>>> GetAll()
    {
        return Ok(await _mediator.Send(new GetSpecialtiesQuery()));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SpecialtyDto>> GetById(int id)
    {
        return Ok(await _mediator.Send(new GetSpecialtyByIdQuery(id)));
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Secretary,Dean")]
    public async Task<ActionResult<int>> Create([FromBody] CreateSpecialtyCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Secretary,Dean")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateSpecialtyCommand command)
    {
        command = command with { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Secretary,Dean")]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteSpecialtyCommand(id));
        return NoContent();
    }
}

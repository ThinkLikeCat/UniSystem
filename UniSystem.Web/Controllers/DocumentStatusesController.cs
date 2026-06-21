using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniSystem.Application.DocumentStatuses;

namespace UniSystem.Web.Controllers;

[ApiController]
[Authorize]
[Route("api/document-statuses")]
public class DocumentStatusesController : ControllerBase
{
    private readonly IMediator _mediator;

    public DocumentStatusesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<DocumentStatusDto>>> GetAll()
    {
        return Ok(await _mediator.Send(new GetDocumentStatusesQuery()));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DocumentStatusDto>> GetById(int id)
    {
        return Ok(await _mediator.Send(new GetDocumentStatusByIdQuery(id)));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<int>> Create([FromBody] CreateDocumentStatusCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateDocumentStatusCommand command)
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
        await _mediator.Send(new DeleteDocumentStatusCommand(id));
        return NoContent();
    }
}

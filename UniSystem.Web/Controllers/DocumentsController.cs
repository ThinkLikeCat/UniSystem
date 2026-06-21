using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniSystem.Application.Documents.Commands;
using UniSystem.Application.Documents.Commands.DeanReview;
using UniSystem.Application.Documents.Commands.SecretaryReview;
using UniSystem.Application.Documents.Commands.SendToReview;
using UniSystem.Application.Documents.Queries;

namespace UniSystem.Web.Controllers;

[ApiController]
[Route("api/documents")]
[Authorize]
public class DocumentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public DocumentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Roles = "StudentProfile")]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateDocumentCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DocumentDetailDto>> GetById(Guid id)
    {
        return Ok(await _mediator.Send(new GetDocumentByIdQuery(id)));
    }

    [HttpGet]
    public async Task<ActionResult<List<DocumentListItemDto>>> GetAll(
        [FromQuery] int? documentTypeId,
        [FromQuery] int? statusId,
        [FromQuery] Guid? authorId)
    {
        return Ok(await _mediator.Send(new GetDocumentsQuery(documentTypeId, statusId, authorId)));
    }

    [HttpPut("{id}/dynamic-values")]
    [Authorize(Roles = "StudentProfile")]
    public async Task<IActionResult> UpdateDynamicValues(Guid id, [FromBody] UpdateDocumentDynamicValuesCommand command)
    {
        if (command.DocumentId != id)
            return BadRequest("ID in URL and request body do not match.");

        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPost("{id}/send-to-review")]
    [Authorize(Roles = "StudentProfile")]
    public async Task<IActionResult> SendToReview(Guid id)
    {
        await _mediator.Send(new SendToReviewCommand(id));
        return NoContent();
    }

    [HttpPost("{id}/secretary-review")]
    [Authorize(Roles = "Secretary")]
    public async Task<IActionResult> SecretaryReview(Guid id, [FromBody] SecretaryReviewCommand command)
    {
        if (command.DocumentId != id)
            return BadRequest("ID in URL and request body do not match.");

        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPost("{id}/dean-review")]
    [Authorize(Roles = "Dean")]
    public async Task<IActionResult> DeanReview(Guid id, [FromBody] DeanReviewCommand command)
    {
        if (command.DocumentId != id)
            return BadRequest("Id в URL и теле запроса не совпадают.");

        await _mediator.Send(command);
        return NoContent();
    }
}

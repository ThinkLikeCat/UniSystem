using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniSystem.Application.DocumentTypes;
using UniSystem.Application.Documents.Queries;

namespace UniSystem.Web.Controllers;

[ApiController]
[Route("api/document-types")]
[Authorize]
public class DocumentTypesController : ControllerBase
{
    private readonly IMediator _mediator;

    public DocumentTypesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<DocumentTypeDto>>> GetAll()
    {
        return Ok(await _mediator.Send(new GetDocumentTypesQuery()));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DocumentTypeDto>> GetById(int id)
    {
        return Ok(await _mediator.Send(new GetDocumentTypeByIdQuery(id)));
    }

    [HttpGet("{id}/template")]
    public async Task<ActionResult<TemplatePreviewDto>> GetTemplate(int id)
    {
        return Ok(await _mediator.Send(new GetDocumentTypeTemplateQuery(id)));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<int>> Create([FromBody] CreateDocumentTypeCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateDocumentTypeCommand command)
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
        await _mediator.Send(new DeleteDocumentTypeCommand(id));
        return NoContent();
    }
}

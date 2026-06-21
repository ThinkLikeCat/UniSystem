using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniSystem.Application.Documents.Commands;
using UniSystem.Application.Documents.Queries;

namespace UniSystem.Web.Controllers;

[ApiController]
[Route("api/documents/{documentId}/attachments")]
[Authorize]
public class AttachmentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AttachmentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Roles = "StudentProfile")]
    [RequestSizeLimit(10 * 1024 * 1024)]
    public async Task<ActionResult<Guid>> Upload(Guid documentId, IFormFile file)
    {
        if (file is null || file.Length == 0)
            return BadRequest("File not selected or empty.");

        await using var stream = file.OpenReadStream();
        var id = await _mediator.Send(new UploadAttachmentCommand(documentId, file.FileName, stream, file.Length));
        return Ok(id);
    }

    [HttpGet("{attachmentId}")]
    public async Task<IActionResult> Download(Guid documentId, Guid attachmentId)
    {
        var result = await _mediator.Send(new DownloadAttachmentQuery(attachmentId));
        return File(result.Content, result.ContentType, result.FileName);
    }

    [HttpDelete("{attachmentId}")]
    [Authorize(Roles = "StudentProfile")]
    public async Task<IActionResult> Delete(Guid documentId, Guid attachmentId)
    {
        await _mediator.Send(new DeleteAttachmentCommand(attachmentId));
        return NoContent();
    }
}

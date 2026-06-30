using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniSystem.Application.StaffSubjects;

namespace UniSystem.Web.Controllers;

[ApiController]
[Authorize]
[Route("api/staff-subjects")]
public class StaffSubjectsController : ControllerBase
{
    private readonly IMediator _mediator;

    public StaffSubjectsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{staffId}")]
    public async Task<ActionResult<List<StaffSubjectDto>>> GetByStaff(Guid staffId)
    {
        return Ok(await _mediator.Send(new GetStaffSubjectsQuery(staffId)));
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Secretary,Dean")]
    public async Task<IActionResult> Create([FromBody] CreateStaffSubjectCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{staffId}/{subjectId}")]
    [Authorize(Roles = "Admin,Secretary,Dean")]
    public async Task<IActionResult> Delete(Guid staffId, int subjectId)
    {
        await _mediator.Send(new DeleteStaffSubjectCommand(staffId, subjectId));
        return NoContent();
    }
}

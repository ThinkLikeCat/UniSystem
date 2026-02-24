using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UniSystem.Domain;

namespace UniSystem.Infrastructure;

[ApiController]
[Route("api/Students/{controller}")]
public class StudentController
{
    [HttpGet]
    public string Get() => "";
}
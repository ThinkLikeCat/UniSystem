using System.Text.RegularExpressions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UniSystem.Application.Common.Interfaces;
using UniSystem.Domain.Entities;
using UniSystem.Domain.Exceptions;

namespace UniSystem.Application.Documents.Queries;

public record TemplatePreviewDto(
    int DocumentTypeId,
    string DocumentTypeName,
    Dictionary<string, string> SystemFields,
    List<string> UserFields
);

public record GetDocumentTypeTemplateQuery(int DocumentTypeId) : IRequest<TemplatePreviewDto>;

public class GetDocumentTypeTemplateQueryHandler : IRequestHandler<GetDocumentTypeTemplateQuery, TemplatePreviewDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IUserContext _userContext;

    public GetDocumentTypeTemplateQueryHandler(IApplicationDbContext context, IUserContext userContext)
    {
        _context = context;
        _userContext = userContext;
    }

    public async Task<TemplatePreviewDto> Handle(GetDocumentTypeTemplateQuery request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId ?? throw new DomainException("User is not authenticated.");

        var documentType = await _context.DocumentTypes
            .FirstOrDefaultAsync(t => t.Id == request.DocumentTypeId, cancellationToken);

        if (documentType is null)
            throw new DomainException("Document type not found.");

        var studentData = await _context.StudentProfiles
            .Where(p => p.Id == userId)
            .Select(p => new
            {
                FullName = p.User.FullName,
                GroupName = p.AcademicGroup != null ? p.AcademicGroup.Name.Value : null,
                Course = p.AcademicGroup != null ? (int?)p.AcademicGroup.Course.Value : null,
                Specialty = p.AcademicGroup != null ? p.AcademicGroup.Specialty.Name.Value : null
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (studentData is null)
        {
            var staffData = await _context.StaffProfiles
                .Where(p => p.Id == userId)
                .Select(p => new
                {
                    FullName = p.User.FullName,
                    GroupName = (string?)null,
                    Course = (int?)null,
                    Specialty = (string?)null
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (staffData is null)
                throw new DomainException("User not found.");

            studentData = staffData;
        }

        var systemValues = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "{student_name}", studentData.FullName ?? string.Empty },
            { "{group_name}",   studentData.GroupName ?? string.Empty },
            { "{course}",       studentData.Course?.ToString() ?? string.Empty },
            { "{specialty}",    studentData.Specialty ?? string.Empty }
        };

        var matches = Regex.Matches(documentType.TemplateText.Value, @"\{[a-zA-Z0-9_]+\}");
        var systemFields = new Dictionary<string, string>();
        var userFields = new List<string>();

        foreach (Match match in matches)
        {
            var placeholder = match.Value;
            if (systemValues.TryGetValue(placeholder, out var systemValue))
                systemFields[placeholder] = systemValue;
            else
                userFields.Add(placeholder);
        }

        return new TemplatePreviewDto(
            request.DocumentTypeId,
            documentType.Name.Value,
            systemFields,
            userFields
        );
    }
}

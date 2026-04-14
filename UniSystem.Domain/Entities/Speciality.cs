using UniSystem.Domain.Common;
using UniSystem.Domain.ValueObjects.Id;

namespace UniSystem.Domain.Entities;

public class Speciality: Entity<SpecialityId>
{
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public int MaxCourse { get; private set; } = 4;

    public List<UniversitySubjectId> UniversitySubjectsId { get; private set; } = new();
}
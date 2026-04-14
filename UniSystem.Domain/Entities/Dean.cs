using UniSystem.Domain.Common;
using UniSystem.Domain.ValueObjects.Id;

namespace UniSystem.Domain.Entities;

public class Dean: Member<DeanId>
{
    private Dean(string fullName, DateTime birthDate, string email, string password)
        : base(fullName, birthDate, email, password)
    {
        
    }
}
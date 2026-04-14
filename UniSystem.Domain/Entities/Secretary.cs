using UniSystem.Domain.Common;
using UniSystem.Domain.ValueObjects.Id;

namespace UniSystem.Domain.Entities;

public class Secretary : Member<SecretaryId>
{
    public Secretary(string fullName, DateTime birthDate, string email, string password)
        : base(fullName, birthDate, email, password)
    {
    }
}
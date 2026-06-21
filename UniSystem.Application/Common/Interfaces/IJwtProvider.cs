namespace UniSystem.Application.Common.Interfaces;

using UniSystem.Domain.Entities;

public interface IJwtProvider
{
    string GenerateToken(User user, IList<string> roles);
}

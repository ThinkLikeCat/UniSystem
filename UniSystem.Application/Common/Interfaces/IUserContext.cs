namespace UniSystem.Application.Common.Interfaces;

public interface IUserContext
{
    Guid? UserId { get; }
    IList<string> Roles { get; }
}

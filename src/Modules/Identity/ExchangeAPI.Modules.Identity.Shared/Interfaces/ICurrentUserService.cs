namespace ExchangeAPI.Modules.Identity.Shared.Interfaces;

public interface ICurrentUserService
{
    string? UserId { get; }
    string? UserEmail { get; }
    string? FullUserName { get; }
    string[]? UserRoles { get; }
}
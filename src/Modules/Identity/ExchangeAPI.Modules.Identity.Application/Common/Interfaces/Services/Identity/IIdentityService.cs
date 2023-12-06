using ExchangeAPI.Shared.Common.Models.Result.Abstractions;
using ExchangeAPI.Shared.Common.Models.Result.Abstractions.Generics;

namespace ExchangeAPI.Modules.Identity.Application.Common.Interfaces.Services.Identity;

public interface IIdentityService
{
    Task<string> GetUserNameAsync(string userId);

    Task<bool> IsInRoleAsync(string userId, string role);

    Task<bool> AuthorizeAsync(string userId, string policyName);

    Task<IResult<string>> CreateUserAsync(string userName, string password);

    Task<IResult> DeleteUserAsync(string userId);
}
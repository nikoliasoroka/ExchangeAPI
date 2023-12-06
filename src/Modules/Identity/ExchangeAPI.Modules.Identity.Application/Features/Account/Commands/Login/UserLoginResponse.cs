using ExchangeAPI.Shared.Common.Models;

namespace ExchangeAPI.Modules.Identity.Application.Features.Account.Commands.Login;

public class UserLoginResponse
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public bool TwoFactorConfigured { get; set; }
}
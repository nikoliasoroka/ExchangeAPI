using ExchangeAPI.Modules.Identity.Domain.Common.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ExchangeAPI.Modules.Identity.Domain.Entities;

public class AppUser : IdentityUser<string>, IBaseEntity<string>
{
    public AppUser()
    {
        Id = Guid.NewGuid().ToString();
        LastModifiedDate = DateTime.UtcNow;
        UserTokens = new HashSet<UserToken>();
        UserClaims = new HashSet<UserClaim>();
        UserLogins = new HashSet<UserLogin>();
        UserRoles = new HashSet<UserRole>();
    }

    public string? FullName { get; set; }
    public string? Firstname { get; set; }
    public string? FullUsername { get; set; }
    public bool IsBlocked { get; set; }
    public DateTime LastActivityTime { get; set; }
    public DateTime LastModifiedDate { get; set; }
    public DateTime? LockoutEndDateUtc { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public DateTime? TwoFactorEnd { get; set; }
    public virtual ICollection<UserClaim> UserClaims { get; set; }
    public virtual ICollection<UserLogin> UserLogins { get; set; }
    public virtual ICollection<UserRole> UserRoles { get; set; }
    public virtual ICollection<UserToken> UserTokens { get; set; }

    public string GetFullName() => $"{Firstname} {FullName}";
}
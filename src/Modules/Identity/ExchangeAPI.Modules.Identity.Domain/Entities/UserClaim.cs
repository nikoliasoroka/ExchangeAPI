using Microsoft.AspNetCore.Identity;

namespace ExchangeAPI.Modules.Identity.Domain.Entities;

public class UserClaim : IdentityUserClaim<string>
{
    public virtual AppUser User { get; set; }
}
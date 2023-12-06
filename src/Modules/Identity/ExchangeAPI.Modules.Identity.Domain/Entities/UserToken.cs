using Microsoft.AspNetCore.Identity;

namespace ExchangeAPI.Modules.Identity.Domain.Entities;

public class UserToken : IdentityUserToken<string>
{
    public DateTime? ExpiryTime { get; set; }
    public virtual AppUser User { get; set; }
}
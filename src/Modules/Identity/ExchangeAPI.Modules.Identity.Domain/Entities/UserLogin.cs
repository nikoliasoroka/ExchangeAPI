using Microsoft.AspNetCore.Identity;

namespace ExchangeAPI.Modules.Identity.Domain.Entities;

public class UserLogin : IdentityUserLogin<string>
{
    public virtual AppUser User { get; set; }
}
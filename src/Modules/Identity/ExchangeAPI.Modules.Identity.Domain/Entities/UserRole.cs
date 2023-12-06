using Microsoft.AspNetCore.Identity;

namespace ExchangeAPI.Modules.Identity.Domain.Entities;

public class UserRole : IdentityUserRole<string>
{
    public UserRole()
    {
    }

    public virtual AppUser User { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;
}
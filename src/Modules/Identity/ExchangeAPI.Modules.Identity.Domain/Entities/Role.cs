using ExchangeAPI.Modules.Identity.Domain.Common.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ExchangeAPI.Modules.Identity.Domain.Entities;

public class Role : IdentityRole<string>, IBaseEntity<string>
{
    public Role()
    {
        Id = Guid.NewGuid().ToString();
        ConcurrencyStamp = Guid.NewGuid().ToString();
        AccessRights = new List<AccessRight>();
    }

    public Role(string name, string displayName, Type? objectType = null)
        : this()
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentNullException(nameof(name));

        Name = name;
        NormalizedName = name.ToUpper();

        if (string.IsNullOrEmpty(displayName))
            throw new ArgumentNullException(nameof(displayName));

        DisplayName = displayName;
    }

    public string? DisplayName { get; set; }
    public virtual ICollection<UserRole> UserRoles { get; set; }
    public virtual List<AccessRight> AccessRights { get; set; }
}
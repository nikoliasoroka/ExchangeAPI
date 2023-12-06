using ExchangeAPI.Modules.Identity.Domain.Entities;

namespace ExchangeAPI.Modules.Identity.Domain.Common.Comparators;

public class RoleEqualityComparer : IEqualityComparer<Role>
{
    public bool Equals(Role x, Role y)
    {
        return string.Equals(x.Name, y.Name, StringComparison.OrdinalIgnoreCase);
    }

    public int GetHashCode(Role obj)
    {
        return obj.Name.ToUpperInvariant().GetHashCode();
    }
}
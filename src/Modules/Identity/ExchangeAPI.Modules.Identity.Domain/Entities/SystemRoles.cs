using System.Reflection;

namespace ExchangeAPI.Modules.Identity.Domain.Entities;

public static class SystemRoles
{
    private const string ADMINISTRATOR = "SuperAdmin";

    public static readonly Role Administrator = new(ADMINISTRATOR, "Administrator");
    public static readonly Role CompanyManager = new("CompanyManager", "Company Manager");

    public static IEnumerable<Role> All => typeof(SystemRoles)
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Where(field => field.FieldType == typeof(Role))
            .Select(field => (Role)field.GetValue(null));

    public static IEnumerable<Role> SuperAdmins => new List<Role>() { Administrator };
}
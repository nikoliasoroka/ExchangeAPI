using ExchangeAPI.Shared.Common.Constants;
using System.Reflection;

namespace ExchangeAPI.Modules.Identity.Domain.Entities;

public static class ActionTypes
{
    public static readonly ActionType ManageUsers = new(SharedActionTypes.ManageUsers, "Manage Users");
    public static readonly ActionType ManageRoles = new(SharedActionTypes.ManageRoles, "Manage Roles");
    public static readonly ActionType LoginMobile = new(SharedActionTypes.LoginMobile, "Login to Mobile");
    public static readonly ActionType LoginBackend = new(SharedActionTypes.LoginBackend, "Login to Backend");
    public static readonly ActionType ViewConfig = new(SharedActionTypes.ViewConfig, "View config");

    public static IEnumerable<ActionType> All => typeof(ActionTypes)
        .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
        .Where(field => field.FieldType == typeof(ActionType))
        .Select(field => (ActionType)field.GetValue(null));

    public static ActionType GetByName(string name)
    {
        var result = All.FirstOrDefault(x => x.Name.Equals(name));

        return result ?? throw new Exception($"Action type '{name}' not found");
    }

    public static List<ActionType> GetByNames(string[] names)
    {
        return All.Where(x => names.Contains(x.Name)).ToList();
    }
}
namespace ExchangeAPI.Modules.Identity.Domain.Entities;

public class AccessRight
{
    public string RoleId { get; set; }
    public Role Role { get; set; }

    public int ActionTypeId { get; set; }
    public ActionType ActionType { get; set; }
}
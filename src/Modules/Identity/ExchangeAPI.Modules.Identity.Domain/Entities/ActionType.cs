using ExchangeAPI.Modules.Identity.Domain.Common.Interfaces;

namespace ExchangeAPI.Modules.Identity.Domain.Entities;

public class ActionType : IBaseEntity<int>
{
    public ActionType()
    { }

    public ActionType(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is empty", "name");

        Name = name;
        Description = description;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }

    public virtual ICollection<AccessRight> AccessRights { get; protected set; }
}
using ExchangeAPI.Modules.Identity.Domain.Common.Interfaces;

namespace ExchangeAPI.Modules.Identity.Domain.Entities;

public class Activity : IBaseEntity<int>
{
    public Activity() { }

    public Activity(string username, string category, string result)
    {
        Username = username;
        Category = category;
        Result = result;
        Date = DateTime.UtcNow;
    }

    public int Id { get; set; }
    public string? Username { get; set; }
    public string? Category { get; set; }
    public string? Result { get; set; }
    public DateTime Date { get; set; }
}
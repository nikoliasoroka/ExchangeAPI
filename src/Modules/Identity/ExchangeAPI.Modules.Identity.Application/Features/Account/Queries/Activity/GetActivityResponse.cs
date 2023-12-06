namespace ExchangeAPI.Modules.Identity.Application.Features.Account.Queries.Activity;

public class GetActivityResponse
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Category { get; set; }
    public string Result { get; set; }
    public DateTime Date { get; set; }
}
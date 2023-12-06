namespace ExchangeAPI.Modules.Identity.Application.Features.Convert.Queries;

public class ConvertResponse
{
    public string From { get; set; }
    public string To { get; set; }
    public decimal Value { get; set; }
    public decimal Converted { get; set; }
}

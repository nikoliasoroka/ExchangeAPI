namespace ExchangeAPI.Modules.Identity.Shared.DTOs;

public class OpenExchangeRateError
{
    public bool Error { get; set; }
    public int Status { get; set; }
    public string Message { get; set; }
    public string Description { get; set; }
}
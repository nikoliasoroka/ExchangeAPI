namespace ExchangeAPI.Shared.Common.Models;

public class LookupDto<T>
{
    public T Id { get; set; }
    public string Name { get; set; }
}

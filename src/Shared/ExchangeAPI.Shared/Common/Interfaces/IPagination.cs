namespace ExchangeAPI.Shared.Common.Interfaces;

public interface IPagination
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
using ExchangeAPI.Shared.Common.Interfaces;

namespace ExchangeAPI.Shared.Common.Models;

public class PagedRequest : IPagination, ISearchFilter
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Search { get; set; }
}
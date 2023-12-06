using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ExchangeAPI.Shared.Common.Models;

public class PaginatedList<TResult> where TResult : class
{
    public int PageNumber { get; }
    public int PageSize { get; }
    public int TotalRecords { get; }
    public List<TResult> Records { get; } = new();

    public PaginatedList(IEnumerable<TResult> items, int count, int pageNumber, int pageSize)
    {
        PageSize = pageSize;
        PageNumber = pageNumber;
        TotalRecords = count;
        Records.AddRange(items);
    }

    public static async Task<PaginatedList<TResult>> CreateAsync<TSource>(IMapper mapper, IQueryable<TSource> source, int pageNumber, int pageSize)
    {
        var count = await source.CountAsync();
        var query = source.Skip((pageNumber - 1) * pageSize).Take(pageSize);

        var records = await mapper.ProjectTo<TResult>(query, null).ToListAsync();

        return new PaginatedList<TResult>(records, count, pageNumber, pageSize);
    }

    public static async Task<PaginatedList<TResult>> CreateAsync(IQueryable<TResult> source, int pageNumber, int pageSize)
    {
        var count = await source.CountAsync();
        var records = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PaginatedList<TResult>(records, count, pageNumber, pageSize);
    }

    public static PaginatedList<TResult> Create(IEnumerable<TResult> records, int pageNumber, int pageSize, int count)
        => new(records, count, pageNumber, pageSize);

    public static PaginatedList<TResult> Empty(int pageNumber, int pageSize)
        => new(Enumerable.Empty<TResult>(), 0, pageNumber, pageSize);
}
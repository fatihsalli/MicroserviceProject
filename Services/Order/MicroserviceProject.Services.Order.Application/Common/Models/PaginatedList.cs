using Microsoft.EntityFrameworkCore;

namespace MicroserviceProject.Services.Order.Application.Common.Models;

/// <summary>
/// Generic olarak oluşturulan "PaginatedList" sınıfımız sayfalama yaparak ya da sayfalama yapmadan response modellerimizi oluşturmamızı sağlar. Sayfalama yapmamamıza rağmen bu modeli kullanma nedenimiz "TotalCount" değerini response içerisinde dönmek içindir.
/// </summary>
/// <typeparam name="T"></typeparam>
public class PaginatedList<T>
{
    public int TotalCount { get; }
    public IReadOnlyCollection<T> Items { get; }
    public int PageNumber { get; }
    public int TotalPages { get; }
    
    public PaginatedList(IReadOnlyCollection<T> items, int count, int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        TotalPages = count <= 0 ? 1 : (int)Math.Ceiling(count / (double)pageSize);
        TotalCount = count;
        Items = items;
    }

    public bool HasPreviousPage => PageNumber > 1;

    public bool HasNextPage => PageNumber < TotalPages;

    public static async Task<PaginatedList<T>> CreateWithPaginatedAsync(IQueryable<T> source, int pageNumber, int pageSize)
    {
        var count = await source.CountAsync();
        var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        return new PaginatedList<T>(items, count, pageNumber, pageSize);
    }
    
    public static async Task<PaginatedList<T>> CreateWithoutPaginatedAsync(IQueryable<T> source)
    {
        var count = await source.CountAsync();
        var items = await source.ToListAsync();
        return new PaginatedList<T>(items, count, 1, count);
    }
}
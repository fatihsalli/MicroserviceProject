using AutoMapper;
using AutoMapper.QueryableExtensions;
using MicroserviceProject.Services.Order.Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroserviceProject.Services.Order.Application.Common.Mappings;

public static class MappingExtensions
{
    public static Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(this IQueryable<TDestination> queryable, int pageNumber, int pageSize) where TDestination : class
        => PaginatedList<TDestination>.CreateWithPaginatedAsync(queryable.AsNoTracking(), pageNumber, pageSize);
    
    public static Task<PaginatedList<TDestination>> PaginatedAllListAsync<TDestination>(this IQueryable<TDestination> queryable) where TDestination : class
        => PaginatedList<TDestination>.CreateWithoutPaginatedAsync(queryable.AsNoTracking());

    public static Task<List<TDestination>> ProjectToListAsync<TDestination>(this IQueryable queryable, IConfigurationProvider configuration) where TDestination : class
        => queryable.ProjectTo<TDestination>(configuration).AsNoTracking().ToListAsync();
}

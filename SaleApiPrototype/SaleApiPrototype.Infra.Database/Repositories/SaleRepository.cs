using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SaleApiPrototype.Domain.Entities;
using SaleApiPrototype.Domain.Models;
using SaleApiPrototype.Domain.Repositories;
using SaleApiPrototype.Infra.Database.DataContext;
using SaleApiPrototype.Infra.Database.Entities;

namespace SaleApiPrototype.Infra.Database.Repositories;

internal class SaleRepository(
    AppDataContext context,
    IMapper mapper)
    : ISaleRepository
{
    public Task<Sale?> GetByCodeAsync(long saleCode) =>
        GetByCodeQuery(saleCode)
        .ProjectTo<Sale>(mapper.ConfigurationProvider)
        .FirstOrDefaultAsync();

    private IQueryable<SaleDb> GetByCodeQuery(long saleCode) =>
        context.Sales
        .Include(s => s.Products)
        .Where(s => s.SaleNumber == saleCode);

    public async Task<Pagination<Sale>> GetPaginatedAsync(int page, int quantityPerPage)
    {
        var baseQuery = context.Sales
                .AsNoTracking()
                .Include(e => e.Products)
                .AsQueryable();

        var count = await baseQuery.CountAsync();
        var results = await baseQuery
            .Skip((page - 1) * quantityPerPage)
            .Take(quantityPerPage)
            .ProjectTo<Sale>(mapper.ConfigurationProvider)
            .ToArrayAsync();

        return new Pagination<Sale>
        {
            CurrentPage = page,
            ResultsPerPage = quantityPerPage,
            TotalPages = count / quantityPerPage,
            TotalResults = count,
            Results = results
        };
    }

    public async Task<Sale> SaveAsync(Sale sale)
    {
        var saleDb = mapper.Map<SaleDb>(sale);
        var entry = context.Sales.Add(saleDb);
        await context.SaveChangesAsync();
        return mapper.Map<Sale>(entry.Entity);
    }

    public async Task<Sale> UpdateAsync(Sale sale)
    {
        var saleDb = await GetByCodeQuery(sale.SaleNumber).FirstAsync();
        saleDb.Customer = sale.Customer;
        saleDb.TotalAmount = sale.TotalAmount;
        saleDb.Branch = sale.Branch;
        saleDb.IsCancelled = sale.IsCancelled;
        saleDb.Products = [];
        context.Sales.Update(saleDb);

        saleDb.Products = mapper.Map<IEnumerable<SaleProductDb>>(sale.Products);
        context.Sales.Update(saleDb);

        await context.SaveChangesAsync();

        return mapper.Map<Sale>(saleDb);
    }
}

using SaleApiPrototype.Domain.Entities;
using SaleApiPrototype.Domain.Models;

namespace SaleApiPrototype.Domain.Repositories;

public interface ISaleRepository
{
    Task<Sale> SaveAsync(Sale sale);
    Task<Sale> UpdateAsync(Sale sale);
    Task<Sale?> GetByCodeAsync(long saleCode);
    Task<Pagination<Sale>> GetPaginatedAsync(int page, int quantityPerPage);
}

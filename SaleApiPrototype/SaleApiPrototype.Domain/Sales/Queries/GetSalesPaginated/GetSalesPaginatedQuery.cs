using MediatR;
using SaleApiPrototype.Domain.Entities;
using SaleApiPrototype.Domain.Models;

namespace SaleApiPrototype.Domain.Sales.Queries.GetSalesPaginated;

public class GetSalesPaginatedQuery : IRequest<Result<Pagination<Sale>>>
{
    public int Page { get; set; }
    public int QuantityPerPage { get; set; }
}

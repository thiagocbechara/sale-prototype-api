using MediatR;
using SaleApiPrototype.Domain.Entities;
using SaleApiPrototype.Domain.Models;

namespace SaleApiPrototype.Domain.Sales.Queries.GetSaleById;

public class GetSaleByIdQuery : IRequest<Result<Sale>>
{
    public long SaleCode { get; set; }
}

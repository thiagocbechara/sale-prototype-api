using MediatR;
using SaleApiPrototype.Domain.Models;

namespace SaleApiPrototype.Domain.SaleProducts.CancelSaleProduct;

public class CancelSaleProductCommand : IRequest<Result>
{
    public long SaleNumber { get; set; }
    public string ProductName { get; set; } = default!;
}

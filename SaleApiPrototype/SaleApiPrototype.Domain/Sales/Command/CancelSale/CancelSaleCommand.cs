using MediatR;
using SaleApiPrototype.Domain.Models;

namespace SaleApiPrototype.Domain.Sales.Command.CancelSale;

public class CancelSaleCommand : IRequest<Result>
{
    public long SaleCode { get; set; }
}

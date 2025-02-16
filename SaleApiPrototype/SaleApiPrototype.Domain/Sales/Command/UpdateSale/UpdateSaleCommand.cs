using MediatR;
using SaleApiPrototype.Domain.Dtos;
using SaleApiPrototype.Domain.Models;

namespace SaleApiPrototype.Domain.Sales.Command.UpdateSale;

public class UpdateSaleCommand : IRequest<Result>
{
    public long SaleCode { get; set; }
    public UpdateSaleDto Sale { get; set; } = default!;
}

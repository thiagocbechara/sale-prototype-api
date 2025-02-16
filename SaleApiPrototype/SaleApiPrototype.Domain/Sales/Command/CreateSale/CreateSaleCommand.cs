using MediatR;
using SaleApiPrototype.Domain.Dtos;
using SaleApiPrototype.Domain.Models;

namespace SaleApiPrototype.Domain.Sales.Command.CreateSale;

public class CreateSaleCommand : IRequest<Result<long>>
{
    public CreateSaleDto Sale { get; set; } = default!;
}

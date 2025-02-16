using MediatR;
using SaleApiPrototype.Domain.Entities;
using SaleApiPrototype.Domain.Enums;
using SaleApiPrototype.Domain.Models;
using SaleApiPrototype.Domain.Repositories;

namespace SaleApiPrototype.Domain.SaleProducts.CancelSaleProduct;

internal class CancelSaleProductCommandHandler(
    ISaleRepository saleRepository)
    : IRequestHandler<CancelSaleProductCommand, Result>
{
    public async Task<Result> Handle(CancelSaleProductCommand request, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Result.Error("Cancellation was requested", DomainErrorType.CancellationToken);
        }

        var sale = await saleRepository.GetByCodeAsync(request.SaleNumber);
        if (sale is null)
        {
            return Result.NotFoundError<Sale>();
        }

        bool ProductNameComparison(SaleProduct p) =>
            !string.Equals(p.ProductName, request.ProductName, StringComparison.OrdinalIgnoreCase);

        sale.Products = sale.Products
            .Where(ProductNameComparison)
            .ToList();

        await saleRepository.UpdateAsync(sale);
        return Result.Success();
    }
}

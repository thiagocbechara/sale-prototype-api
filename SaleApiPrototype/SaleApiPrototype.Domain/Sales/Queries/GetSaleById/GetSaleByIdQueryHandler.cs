using MediatR;
using SaleApiPrototype.Domain.Entities;
using SaleApiPrototype.Domain.Enums;
using SaleApiPrototype.Domain.Models;
using SaleApiPrototype.Domain.Repositories;

namespace SaleApiPrototype.Domain.Sales.Queries.GetSaleById;

internal class GetSaleByIdQueryHandler(
    ISaleRepository repository)
    : IRequestHandler<GetSaleByIdQuery, Result<Sale>>
{
    public async Task<Result<Sale>> Handle(GetSaleByIdQuery request, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Result.Error<Sale>("Cancellation was requested", DomainErrorType.CancellationToken);
        }

        var sale = await repository.GetByCodeAsync(request.SaleCode);
        if (sale is null)
        {
            return Result.NotFoundError<Sale>();
        }

        return Result.Success(sale);
    }
}

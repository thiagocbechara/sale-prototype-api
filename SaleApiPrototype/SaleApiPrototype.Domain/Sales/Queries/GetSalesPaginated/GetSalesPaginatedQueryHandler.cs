using MediatR;
using SaleApiPrototype.Domain.Entities;
using SaleApiPrototype.Domain.Enums;
using SaleApiPrototype.Domain.Models;
using SaleApiPrototype.Domain.Repositories;

namespace SaleApiPrototype.Domain.Sales.Queries.GetSalesPaginated;

internal class GetSalesPaginatedQueryHandler(
    ISaleRepository repository)
    : IRequestHandler<GetSalesPaginatedQuery, Result<Pagination<Sale>>>
{
    public async Task<Result<Pagination<Sale>>> Handle(GetSalesPaginatedQuery request, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Result.Error<Pagination<Sale>>("Cancellation was requested", DomainErrorType.CancellationToken);
        }

        var results = await repository.GetPaginatedAsync(request.Page, request.QuantityPerPage);
        return Result.Success(results);

    }
}

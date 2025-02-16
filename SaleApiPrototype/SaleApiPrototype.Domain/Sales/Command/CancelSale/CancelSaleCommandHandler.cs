using MediatR;
using SaleApiPrototype.Domain.Entities;
using SaleApiPrototype.Domain.Enums;
using SaleApiPrototype.Domain.MessageBroker;
using SaleApiPrototype.Domain.MessageBroker.Events;
using SaleApiPrototype.Domain.Models;
using SaleApiPrototype.Domain.Repositories;

namespace SaleApiPrototype.Domain.Sales.Command.CancelSale;

internal class CancelSaleCommandHandler(
    ISaleRepository repository,
    IQueuePublisher publisher)
    : IRequestHandler<CancelSaleCommand, Result>
{
    public async Task<Result> Handle(CancelSaleCommand request, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Result.Error("Cancellation was requested", DomainErrorType.CancellationToken);
        }

        var sale = await repository.GetByCodeAsync(request.SaleCode);
        if (sale is null)
        {
            return Result.NotFoundError<Sale>();
        }

        if (sale.IsCancelled)
        {
            return Result.Error("Cannot cancel a cancelled sale", DomainErrorType.NotValid);
        }

        sale.IsCancelled = true;
        await repository.UpdateAsync(sale);
        await publisher.PublishAsync<ISaleCancelledEvent>(sale, cancellationToken);

        return Result.Success();
    }
}

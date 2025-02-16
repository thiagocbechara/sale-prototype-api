using AutoMapper;
using FluentValidation;
using MediatR;
using SaleApiPrototype.Domain.Entities;
using SaleApiPrototype.Domain.Enums;
using SaleApiPrototype.Domain.MessageBroker;
using SaleApiPrototype.Domain.MessageBroker.Events;
using SaleApiPrototype.Domain.Models;
using SaleApiPrototype.Domain.Repositories;

namespace SaleApiPrototype.Domain.Sales.Command.CreateSale;

internal class CreateSaleCommandHandler(
    ISaleRepository repository,
    IValidator<Sale> saleValidator,
    IMapper mapper,
    IQueuePublisher publisher)
    : IRequestHandler<CreateSaleCommand, Result<long>>
{
    public async Task<Result<long>> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Result.Error<long>("Cancellation was requested", DomainErrorType.CancellationToken);
        }

        var sale = mapper.Map<Sale>(request.Sale);
        var saleValidate = saleValidator.Validate(sale);
        if (!saleValidate.IsValid)
        {
            var erros = saleValidate.Errors.Select(e => e.ErrorMessage);
            var errorMessage = string.Join(Environment.NewLine, erros);
            return Result.Error<long>($"Sale isn't valid. {errorMessage}", DomainErrorType.NotValid);
        }

        sale.WhenWasMade = DateTime.UtcNow;
        sale.ApplyDiscountRule();

        var saleSaved = await repository.SaveAsync(sale);

        await publisher.PublishAsync<ISaleCreatedEvent>(saleSaved, cancellationToken);

        return Result.Success(saleSaved.SaleNumber);
    }
}

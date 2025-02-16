using AutoMapper;
using FluentValidation;
using MediatR;
using SaleApiPrototype.Domain.Entities;
using SaleApiPrototype.Domain.Enums;
using SaleApiPrototype.Domain.MessageBroker;
using SaleApiPrototype.Domain.MessageBroker.Events;
using SaleApiPrototype.Domain.Models;
using SaleApiPrototype.Domain.Repositories;

namespace SaleApiPrototype.Domain.Sales.Command.UpdateSale;

public class UpdateSaleCommandHandler(
    ISaleRepository repository,
    IMapper mapper,
    IValidator<Sale> saleValidator,
    IQueuePublisher publisher)
    : IRequestHandler<UpdateSaleCommand, Result>
{
    public async Task<Result> Handle(UpdateSaleCommand request, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Result.Error("Cancellation was requested", DomainErrorType.CancellationToken);
        }

        var saleInput = mapper.Map<Sale>(request.Sale);
        var saleValidate = saleValidator.Validate(saleInput);
        if (!saleValidate.IsValid)
        {
            var erros = saleValidate.Errors.Select(e => e.ErrorMessage);
            var errorMessage = string.Join(Environment.NewLine, erros);
            return Result.Error($"Sale isn't valid. {errorMessage}", DomainErrorType.NotValid);
        }

        var saleDb = await repository.GetByCodeAsync(request.SaleCode);
        if (saleDb is null)
        {
            return Result.NotFoundError<Sale>();
        }
        if (saleDb.IsCancelled)
        {
            return Result.Error("Cannot update a cancelled sale", DomainErrorType.NotValid);
        }

        UpdateSaleData(saleDb, saleInput);

        await repository.UpdateAsync(saleDb);
        await publisher.PublishAsync<ISaleModifiedEvent>(saleDb, cancellationToken);

        return Result.Success();
    }

    private static void UpdateSaleData(Sale saleDb, Sale input)
    {
        saleDb.Customer = input.Customer;
        saleDb.Branch = input.Branch;
        saleDb.Products = [.. input.Products];
        saleDb.ApplyDiscountRule();
    }
}

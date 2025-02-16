using FluentValidation;
using SaleApiPrototype.Domain.Entities;

namespace SaleApiPrototype.Domain.Validations;

internal class SaleProductValidator : AbstractValidator<SaleProduct>
{
    public SaleProductValidator()
    {
        RuleFor(p => p.ProductName).NotEmpty();
        RuleFor(p => p.UnitPrice).GreaterThan(0);
        RuleFor(p => p.Quantity)
            .GreaterThan(0)
            .Custom((value, context) =>
            {
                if(value > 20)
                {
                    context.AddFailure("It’s not possible to sell above 20 same item");
                }
            });
    }
}

using FluentValidation;
using SaleApiPrototype.Domain.Entities;

namespace SaleApiPrototype.Domain.Validations
{
    internal class SaleValidator : AbstractValidator<Sale>
    {
        public SaleValidator(IValidator<SaleProduct> productValidator)
        {
            RuleFor(s => s.Customer).NotEmpty();
            RuleFor(s => s.Branch).NotEmpty();
            RuleFor(s => s.Products)
                .NotEmpty()
                .Must(products => 
                    !products
                    .GroupBy(p => p.ProductName)
                    .Where(g => g.Count() > 1)
                    .Any());
            RuleForEach(s => s.Products).SetValidator(productValidator);
        }
    }
}

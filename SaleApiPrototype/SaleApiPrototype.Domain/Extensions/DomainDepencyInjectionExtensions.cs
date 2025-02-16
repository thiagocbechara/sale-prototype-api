using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SaleApiPrototype.Domain.Entities;
using SaleApiPrototype.Domain.MapperProfile;
using SaleApiPrototype.Domain.Sales.Command.CancelSale;
using SaleApiPrototype.Domain.Validations;

namespace SaleApiPrototype.Domain.Extensions;

public static class DomainDepencyInjectionExtensions
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(DomainProfile));
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CancelSaleCommand>());
        services.AddScoped<IValidator<Sale>, SaleValidator>();
        services.AddScoped<IValidator<SaleProduct>, SaleProductValidator>();

        return services;
    }
}

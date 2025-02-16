using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SaleApiPrototype.Domain.Repositories;
using SaleApiPrototype.Infra.Database.DataContext;
using SaleApiPrototype.Infra.Database.MapperProfile;
using SaleApiPrototype.Infra.Database.Repositories;

namespace SaleApiPrototype.Infra.Database.Extensions;

public static class DatabaseDependencyInjectionExtensions
{
    public static IServiceCollection AddDatabaseService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDataContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("SaleApi")));
        services.AddAutoMapper(typeof(DatabaseProfile));

        services.AddScoped<ISaleRepository, SaleRepository>();

        return services;
    }
}

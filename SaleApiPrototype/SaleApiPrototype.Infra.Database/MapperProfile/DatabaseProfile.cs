using AutoMapper;
using SaleApiPrototype.Domain.Entities;
using SaleApiPrototype.Infra.Database.Entities;

namespace SaleApiPrototype.Infra.Database.MapperProfile;

internal class DatabaseProfile : Profile
{
    public DatabaseProfile()
    {
        CreateMap<Sale, SaleDb>().ReverseMap();
        CreateMap<SaleProduct, SaleProductDb>().ReverseMap();
    }
}

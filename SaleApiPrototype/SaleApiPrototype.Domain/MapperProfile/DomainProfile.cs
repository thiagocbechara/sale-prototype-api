using AutoMapper;
using SaleApiPrototype.Domain.Dtos;
using SaleApiPrototype.Domain.Entities;

namespace SaleApiPrototype.Domain.MapperProfile;

internal class DomainProfile : Profile
{
    public DomainProfile()
    {
        CreateMap<CreateSaleDto, Sale>();
        CreateMap<CreateSaleProductDto, SaleProduct>();
    }
}

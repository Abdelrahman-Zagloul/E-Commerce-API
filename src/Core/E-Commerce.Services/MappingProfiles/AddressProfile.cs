using AutoMapper;
using E_Commerce.Domain.Entities.IdentityModule;
using E_Commerce.Shared.DTOs.Authentications;

namespace E_Commerce.Services.MappingProfiles
{
    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<Address, AddressDto>().ReverseMap();
        }
    }
}

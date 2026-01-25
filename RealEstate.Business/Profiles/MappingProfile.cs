using AutoMapper;
using RealEstate.Business.Dtos.PropertyDtos;
using RealEstate.Entity.Concrete;

namespace RealEstate.Business.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // PropertyCreateDto -> Property 
        CreateMap<PropertyCreateDto, Property>();

        // Property -> PropertyCreateDto
        CreateMap<Property, PropertyCreateDto>();

        //PropertyUpdateDto and PropertyListDto will be added here in the future.
    }
}

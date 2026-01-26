using AutoMapper;
using RealEstate.Business.Dtos.PropertyDtos;
using RealEstate.Entity.Concrete;

namespace RealEstate.Business.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // PropertyCreateDto <-> Property 
        CreateMap<PropertyCreateDto, Property>().ReverseMap();

        // PropertyUpdateDto <-> Property 
        CreateMap<PropertyUpdateDto, Property>().ReverseMap();
    }
}

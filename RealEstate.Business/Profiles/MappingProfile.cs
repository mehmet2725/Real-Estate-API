using AutoMapper;
using RealEstate.Business.Dtos.PropertyDtos;
using RealEstate.Business.Dtos.PropertyImageDtos;
using RealEstate.Entity.Concrete;
using RealEstate.Business.Dtos.PropertyTypeDtos;
using RealEstate.Business.Dtos.InquiryDtos;


namespace RealEstate.Business.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // PropertyCreateDto <-> Property 
        CreateMap<PropertyCreateDto, Property>().ReverseMap();

        // PropertyUpdateDto <-> Property 
        CreateMap<PropertyUpdateDto, Property>().ReverseMap();


        // Resim Eşleştirmesi
        CreateMap<PropertyImage, PropertyImageDto>();

        // İlan Eşleştirmesi
        CreateMap<Property, PropertyListDto>()
            // propertyType nesnesinin içindeki Name i al bizim dtodaki "propertyTypeName" e koy
            .ForMember(dest => dest.PropertyTypeName, opt => opt.MapFrom(src => src.PropertyType.Name))
            // resimler otomatik olarak eşleştir
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images));

        // propType eşleştirmeleri
        CreateMap<PropertyType, PropertyTypeDto>().ReverseMap();
        CreateMap<PropertyType, PropertyTypeCreateDto>().ReverseMap();
        CreateMap<PropertyType, PropertyTypeUpdateDto>().ReverseMap();

        // Inquiry (Mesajlaşma)
        CreateMap<InquiryCreateDto, Inquiry>();
        CreateMap<InquiryUpdateDto, Inquiry>();
        CreateMap<Inquiry, InquiryDto>()
            // Mesajın ait olduğu ilanın başlığını da DTO'ya taşıyalım
            .ForMember(dest => dest.PropertyTitle, opt => opt.MapFrom(src => src.Property.Title));
    }
}

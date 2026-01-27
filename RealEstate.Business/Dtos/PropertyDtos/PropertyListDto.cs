using RealEstate.Business.Dtos.PropertyImageDtos;
using System.Collections.Generic;

namespace RealEstate.Business.Dtos.PropertyDtos;

public class PropertyListDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string City { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public int Rooms { get; set; }
    public int Area { get; set; }
    
    public string PropertyTypeName { get; set; }  = string.Empty;
    
    public List<PropertyImageDto> Images { get; set; } = new();
}

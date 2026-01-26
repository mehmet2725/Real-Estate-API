using System;

namespace RealEstate.Business.Dtos.PropertyDtos;

public class PropertyUpdateDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public int Rooms { get; set; }
    public int? Bathrooms { get; set; }
    public decimal Area { get; set; }
    public int Floor { get; set; }
    public int? TotalFloors { get; set; }
    public int YearBuilt { get; set; }

    public string Status { get; set; } = string.Empty;
    public int PropertyTypeId { get; set; }
}

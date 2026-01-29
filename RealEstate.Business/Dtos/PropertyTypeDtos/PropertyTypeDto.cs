using System;

namespace RealEstate.Business.Dtos.PropertyTypeDtos;

public class PropertyTypeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

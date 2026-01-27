using System;

namespace RealEstate.Business.Dtos.PropertyImageDtos;

public class PropertyImageDto
{
    public int Id { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public bool IsPrimary { get; set; }
}

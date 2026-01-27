using Microsoft.AspNetCore.Http;

namespace RealEstate.Business.Dtos.PropertyImageDtos;

public class PropertyImageCreateDto
{
    public int PropertyId { get; set; }
    public IFormFile File { get; set; }
    public bool IsPrimary { get; set; }
}

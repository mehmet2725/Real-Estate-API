using Microsoft.AspNetCore.Mvc;
using RealEstate.Business.Abstract;
using RealEstate.Business.Dtos.PropertyImageDtos;
using System.Threading.Tasks;

namespace RealEstate.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PropertyImagesController : ControllerBase
{
    private readonly IPropertyImageService _propertyImageService;

    public PropertyImagesController(IPropertyImageService propertyImageService)
    {
        _propertyImageService = propertyImageService;
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromForm] PropertyImageCreateDto dto)
    {
        await _propertyImageService.AddAsync(dto);
        return Ok(new { message = "Fotoğraf Başarıyla Yüklendi" });
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Business.Abstract;
using RealEstate.Business.Dtos.PropertyDtos;
using System.Threading.Tasks;

namespace RealEstate.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PropertiesController : ControllerBase
{
    private readonly IPropertyService _propertyService;

    public PropertiesController(IPropertyService propertyService)
    {
        _propertyService = propertyService;
    }

    // GET: api/properties
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var values = await _propertyService.GetAllAsync();
        return Ok(values);
    }

    // POST: api/properties
    [Authorize(Roles = "Admin,Agent")]
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] PropertyCreateDto createDto, [FromQuery] int agentId)
    {
        if (agentId <= 0)
            return BadRequest("Geçerli bir Agent ID giriniz");

        var result = await _propertyService.AddAsync(createDto, agentId);

        return CreatedAtAction(nameof(GetAll), new { id = result.Id }, result);
    }

    // Artık ID'yi URL'den zorunlu istiyoruz [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Agent")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] PropertyUpdateDto updateDto)
    {
        // Güvenlik kontrolü: URL'deki ID ile Kutu içindeki ID aynı mı?
        if (id != updateDto.Id)
        {
            return BadRequest("URL'deki ID ile gönderilen verideki ID uyuşmuyor! İkisini de aynı gir.");
        }

        await _propertyService.UpdateAsync(updateDto);
        return Ok(new { message = "İlan Başarıyla Güncellendi", id = updateDto.Id });
    }

    // DELETE: api/properties/5
    [Authorize(Roles = "Admin,Agent")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _propertyService.DeleteAsync(id);
        return Ok(new { message = "İlan Başarıyla Silindi(Soft Delete)" });
    }

    // Filter
    // GET: api/properties/search?city=Ankara&minPrice=5000&pageNumber=1
    [HttpGet("search")]
    public async Task<IActionResult> GetFiltered([FromQuery] PropertyFilterParams filters)
    {
        var values = await _propertyService.GetFilteredListAsync(filters);
        return Ok(values);
    }
}
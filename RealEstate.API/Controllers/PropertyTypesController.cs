using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Business.Abstract;
using RealEstate.Business.Dtos.PropertyTypeDtos;
using System.Threading.Tasks;

namespace RealEstate.API.Controllers;


[Route("api/[controller]")]
[ApiController]
public class PropertyTypesController : ControllerBase
{
    private readonly IPropertyTypeService _service;

    public PropertyTypesController(IPropertyTypeService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var values = await _service.GetAllAsync();
        return Ok(values);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var value = await _service.GetByIdAsync(id);
        if (value == null) return NotFound("Emlak tipi bulunamadı.");
        return Ok(value);
    }

    // Sadece Adminler ekleme yapabilir
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Add(PropertyTypeCreateDto createDto)
    {
        await _service.AddAsync(createDto);
        return Ok(new { message = "Emlak Tipi başarıyla eklendi." });
    }

    // Sadece Adminler güncelleyebilir
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, PropertyTypeUpdateDto updateDto)
    {
        if (id != updateDto.Id)
            return BadRequest("ID uyuşmazlığı.");

        await _service.UpdateAsync(updateDto);
        return Ok(new { message = "Emlak Tipi güncellendi." });
    }

    // Sadece Adminler silebilir
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return Ok(new { message = "Emlak Tipi silindi (Soft Delete)." });
    }
}

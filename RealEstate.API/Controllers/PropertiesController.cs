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

    // Constructor Injection
    public PropertiesController(IPropertyService propertyService)
    {
        _propertyService = propertyService;
    }

    // GET: api/properties
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var properties = await _propertyService.GetAllAsync();
        return Ok(properties); // 200 OK
    }

    // POST: api/properties?agentId=1
    // Normally, the agentId is obtained from the token, but for now we are getting it from a parameter.
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] PropertyCreateDto createDto, [FromQuery] int agentId)
    {
        // a simple validation
        if(agentId <= 0)
        return BadRequest("Geçerli bir Agen ID giriniz");

        var result = await _propertyService.AddAsync(createDto, agentId);

        // return 201 Created
        return CreatedAtAction(nameof(GetAll), new { id = result.Id }, result);
    }
}

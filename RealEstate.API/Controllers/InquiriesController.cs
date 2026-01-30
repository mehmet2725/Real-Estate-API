using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Business.Abstract;
using RealEstate.Business.Dtos.InquiryDtos;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RealEstate.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InquiriesController : ControllerBase
{
    private readonly IInquiryService _inquiryService;

    public InquiriesController(IInquiryService inquiryService)
    {
        _inquiryService = inquiryService;
    }

    // Mesaj Gönder (Kilit yok, herkes gönderebilir)
    [HttpPost]
    public async Task<IActionResult> SendInquiry(InquiryCreateDto createDto)
    {
        int? userId = null;
        
        // Kullanıcı giriş yapmış mı kontrol et (Token var mı?)
        if (User.Identity != null && User.Identity.IsAuthenticated)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            // Eğer Claim null değilse değerini al
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int parsedId))
            {
                userId = parsedId;
            }
        }

        await _inquiryService.AddAsync(createDto, userId);
        return Ok(new { message = "Mesajınız iletildi." });
    }

    // Mesajları Oku (Sadece Admin veya Agent)
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetMyInquiries()
    {
        // Token içindeki bilgileri güvenli şekilde alalım
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        var roleClaim = User.FindFirst(ClaimTypes.Role);

        // Eğer Token bozuksa veya bilgiler eksikse 401 (Unauthorized) dön
        if (userIdClaim == null || roleClaim == null)
        {
            return Unauthorized("Kimlik bilgileri eksik.");
        }

        var userId = int.Parse(userIdClaim.Value);
        var role = roleClaim.Value;

        var values = await _inquiryService.GetAllAsync(userId, role);
        return Ok(values);
    }

    // Durum Güncelle
    [Authorize]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateStatus(InquiryUpdateDto updateDto)
    {
        await _inquiryService.UpdateStatusAsync(updateDto);
        return Ok(new { message = "Mesaj durumu güncellendi." });
    }
    
    // Sil
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _inquiryService.DeleteAsync(id);
        return Ok(new { message = "Mesaj silindi." });
    }
}
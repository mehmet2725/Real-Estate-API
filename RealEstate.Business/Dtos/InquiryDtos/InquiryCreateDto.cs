using System.ComponentModel.DataAnnotations;

namespace RealEstate.Business.Dtos.InquiryDtos;

public class InquiryCreateDto
{
    [Required]
    public int PropertyId { get; set; } // Hangi ev için?

    [Required(ErrorMessage = "Ad Soyad zorunludur")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "E-posta zorunludur")]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public string? Phone { get; set; }

    [Required(ErrorMessage = "Mesaj içeriği zorunludur")]
    public string Message { get; set; } = string.Empty;

    // Kullanıcı giriş yapmışsa UserId'yi token'dan alacağız, buraya koymuyoruz.
}

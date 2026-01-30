using System;

namespace RealEstate.Business.Dtos.InquiryDtos;

public class InquiryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty; // Yeni, Okundu vs.
    public DateTime CreatedAt { get; set; }

    // Hangi İlan?
    public int PropertyId { get; set; }
    public string PropertyTitle { get; set; } = string.Empty;
}

using System;

namespace RealEstate.Business.Dtos.InquiryDtos;

public class InquiryUpdateDto
{
    public int Id { get; set; }
    public string Status { get; set; } = "Okundu"; // Örn: Okundu, Cevaplandı
}

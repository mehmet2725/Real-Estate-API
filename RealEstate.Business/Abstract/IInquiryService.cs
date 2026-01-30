using System.Collections.Generic;
using System.Threading.Tasks;
using RealEstate.Business.Dtos.InquiryDtos;

namespace RealEstate.Business.Abstract;

public interface IInquiryService
{
    // Mesaj Gönder (Herkes yapabilir)
    Task AddAsync(InquiryCreateDto createDto, int? userId);

    // Mesajları Listele (Rolüne göre değişir)
    Task<List<InquiryDto>> GetAllAsync(int userId, string role);

    // Durum Güncelle (Okundu/Cevaplandı yap)
    Task UpdateStatusAsync(InquiryUpdateDto updateDto);

    // Mesaj Sil
    Task DeleteAsync(int id);
}

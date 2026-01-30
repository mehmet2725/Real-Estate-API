using AutoMapper;
using RealEstate.Business.Abstract;
using RealEstate.Business.Dtos.InquiryDtos;
using RealEstate.Data.Abstract;
using RealEstate.Entity.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstate.Business.Concrete;

public class InquiryManager : IInquiryService
{
    private readonly IGenericRepository<Inquiry> _inquiryRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public InquiryManager(IGenericRepository<Inquiry> inquiryRepo, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _inquiryRepo = inquiryRepo;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task AddAsync(InquiryCreateDto createDto, int? userId)
    {
        var inquiry = _mapper.Map<Inquiry>(createDto);

        inquiry.Status = "Yeni";
        inquiry.UserId = userId; // Eğer giriş yapmışsa ID'si, yapmamışsa null

        await _inquiryRepo.AddAsync(inquiry);
        await _unitOfWork.CommitAsync();
    }

    public async Task<List<InquiryDto>> GetAllAsync(int userId, string role)
    {
        // İlişkili olduğu ilanı (Property) da çekelim ki başlığını görelim
        var allInquiries = await _inquiryRepo.GetAllAsync("Property");
        var activeInquiries = allInquiries.Where(x => !x.IsDeleted).AsQueryable();

        // Eğer ADMIN ise hepsini görür, değilse filtrele
        if (role != "Admin")
        {
            // Sadece bu kullanıcının (Agent'ın) ilanlarına gelen mesajları getir
            activeInquiries = activeInquiries.Where(x => x.Property.AgentId == userId);
        }

        var resultList = activeInquiries.OrderByDescending(x => x.CreatedAt).ToList();
        return _mapper.Map<List<InquiryDto>>(resultList);
    }

    public async Task UpdateStatusAsync(InquiryUpdateDto updateDto)
    {
        var inquiry = await _inquiryRepo.GetByIdAsync(updateDto.Id);
        if (inquiry == null) return;

        inquiry.Status = updateDto.Status; // "Okundu", "Cevaplandı" vs.

        _inquiryRepo.Update(inquiry);
        await _unitOfWork.CommitAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var inquiry = await _inquiryRepo.GetByIdAsync(id);
        if (inquiry == null) return;

        inquiry.IsDeleted = true; // Soft delete
        _inquiryRepo.Update(inquiry);
        await _unitOfWork.CommitAsync();
    }
}

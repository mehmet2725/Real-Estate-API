using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using RealEstate.Business.Abstract;
using RealEstate.Business.Dtos.PropertyDtos;
using RealEstate.Data.Abstract;
using RealEstate.Entity.Concrete;

namespace RealEstate.Business.Concrete;

public class PropertyManager : IPropertyService
{
    private readonly IGenericRepository<Property> _propertyRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PropertyManager(IGenericRepository<Property> propertyRepo, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _propertyRepo = propertyRepo;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Property> AddAsync(PropertyCreateDto createDto, int agentId)
    {
        // 1. Convert DTO to Entity
        var property = _mapper.Map<Property>(createDto);

        // 2. Fill in the missing information manually.
        property.AgentId = agentId;
        property.CreatedAt = DateTime.UtcNow;
        property.IsDeleted = false;

        // 3. Add to Repository(Not yet in database)
        await _propertyRepo.AddAsync(property);

        // 4. Save the database(commit);
        await _unitOfWork.CommitAsync();
        return property;
    }

    public async Task<List<PropertyListDto>> GetAllAsync()
    {
        // Veritabanını çek
        var properties = await _propertyRepo.GetAllAsync("Images", "PropertyType");

        // Sadece silinmeyenleri göster
        var activeProperties = properties.Where(x => x.IsDeleted == false).ToList();

        // Entity -> Dto (AutoMapper)
        var propertyDtos = _mapper.Map<List<PropertyListDto>>(activeProperties);

        return propertyDtos;
    }

    public async Task UpdateAsync(PropertyUpdateDto updateDto)
    {
        var property = await _propertyRepo.GetByIdAsync(updateDto.Id);

        if (property == null) return; // Kayıt yoksa çık

        // (updateDto içindeki verileri al, property'nin içine göm)
        _mapper.Map(updateDto, property);

        //  Kaydet
        _propertyRepo.Update(property);
        await _unitOfWork.CommitAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var property = await _propertyRepo.GetByIdAsync(id);

        if (property == null) return;

        // SOFT DELETE
        property.IsDeleted = true;
        await _unitOfWork.CommitAsync();
    }

    public async Task<List<PropertyListDto>> GetFilteredListAsync(PropertyFilterParams filters)
    {
        // 1. Tüm İlanları İlişkileriyle (Resim, Tip) Çek
        // (Şimdilik Business katmanında EF Core olmadığı için veriyi RAM'e çekip filtreliyoruz. 
        // Veri sayısı azken bu en güvenli ve hatasız yöntemdir.)
        var allProperties = await _propertyRepo.GetAllAsync("Images", "PropertyType");

        // 2. Silinmişleri baştan ele
        var query = allProperties.Where(x => !x.IsDeleted).AsQueryable();

        // 3. Filtreleri Tek Tek Uygula
        if (!string.IsNullOrEmpty(filters.City))
            query = query.Where(x => x.City.ToLower().Contains(filters.City.ToLower()));

        if (!string.IsNullOrEmpty(filters.District))
            query = query.Where(x => x.District.ToLower().Contains(filters.District.ToLower()));

        if (filters.MinPrice.HasValue)
            query = query.Where(x => x.Price >= filters.MinPrice.Value);

        if (filters.MaxPrice.HasValue)
            query = query.Where(x => x.Price <= filters.MaxPrice.Value);

        if (filters.MinRooms.HasValue)
            query = query.Where(x => x.Rooms >= filters.MinRooms.Value);

        if (filters.MaxRooms.HasValue)
            query = query.Where(x => x.Rooms <= filters.MaxRooms.Value);

        if (filters.PropertyTypeId.HasValue)
            query = query.Where(x => x.PropertyTypeId == filters.PropertyTypeId.Value);

        if (!string.IsNullOrEmpty(filters.SearchKeyword))
            query = query.Where(x => x.Title.ToLower().Contains(filters.SearchKeyword.ToLower()) ||
                                     x.Description.ToLower().Contains(filters.SearchKeyword.ToLower()));

        // 4. Sıralama (Switch Expression ile pratik çözüm)
        query = filters.SortBy.ToLower() switch
        {
            "price" => filters.SortOrder == "asc" ? query.OrderBy(x => x.Price) : query.OrderByDescending(x => x.Price),
            "rooms" => filters.SortOrder == "asc" ? query.OrderBy(x => x.Rooms) : query.OrderByDescending(x => x.Rooms),
            "area" => filters.SortOrder == "asc" ? query.OrderBy(x => x.Area) : query.OrderByDescending(x => x.Area),
            _ => filters.SortOrder == "asc" ? query.OrderBy(x => x.CreatedAt) : query.OrderByDescending(x => x.CreatedAt)
        };

        // 5. Sayfalama (Pagination)
        var pagedList = query
            .Skip((filters.PageNumber - 1) * filters.PageSize)
            .Take(filters.PageSize)
            .ToList();

        // 6. DTO'ya Çevir ve Gönder
        return _mapper.Map<List<PropertyListDto>>(pagedList);
    }
}

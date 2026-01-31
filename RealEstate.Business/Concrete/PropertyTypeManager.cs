using AutoMapper;
using RealEstate.Business.Abstract;
using RealEstate.Business.Dtos.PropertyTypeDtos;
using RealEstate.Data.Abstract;
using RealEstate.Entity.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace RealEstate.Business.Concrete;

public class PropertyTypeManager : IPropertyTypeService
{
    private readonly IGenericRepository<PropertyType> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _memoryCache;
    
    // Cache anahtarını sabit olarak tanımlayalım ki her yerde aynısını kullanalım
    private const string CacheKey = "property_types_list";

    public PropertyTypeManager(IGenericRepository<PropertyType> repository, IUnitOfWork unitOfWork, IMapper mapper, IMemoryCache memoryCache)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _memoryCache = memoryCache;
    }

    public async Task<List<PropertyTypeDto>> GetAllAsync()
    {
        // 1. Önce Cache'de veri var mı diye kontrol et
        if (_memoryCache.TryGetValue(CacheKey, out List<PropertyTypeDto> cachedList))
        {
            return cachedList;
        }

        // IsDeleted olmayanları getir
        var values = await _repository.GetAllAsync();
        // Repository'de Where metodumuz var ama GetAllAsync list döndüğü için bellek içi filtreleme yapıyoruz şimdilik.
        // İdealde Repository'e GetListByFilter eklenebilir ama şu an bu yeterli.
        var activeValues = values.Where(x => !x.IsDeleted).ToList();

        // Veriyi hazırla
        cachedList = _mapper.Map<List<PropertyTypeDto>>(activeValues);

        // 2. Cache seçeneklerini ayarla (1 saat hafızada tutsun)
        var cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromHours(1))
            .SetPriority(CacheItemPriority.Normal);

        // 3. Veriyi Cache'e yaz
        _memoryCache.Set(CacheKey, cachedList, cacheOptions);

        return cachedList;
    }

    public async Task<PropertyTypeDto> GetByIdAsync(int id)
    {
        var value = await _repository.GetByIdAsync(id);
        if (value == null || value.IsDeleted) return null;

        return _mapper.Map<PropertyTypeDto>(value);
    }

    public async Task AddAsync(PropertyTypeCreateDto createDto)
    {
        var entity = _mapper.Map<PropertyType>(createDto);
        await _repository.AddAsync(entity);
        await _unitOfWork.CommitAsync();

        // Veri değişti, cache'i temizle
        _memoryCache.Remove(CacheKey);
    }

    public async Task UpdateAsync(PropertyTypeUpdateDto updateDto)
    {
        var entity = await _repository.GetByIdAsync(updateDto.Id);
        if (entity == null) return;

        // updateDto verilerini entity üzerine yaz
        _mapper.Map(updateDto, entity);

        _repository.Update(entity);
        await _unitOfWork.CommitAsync();

        // Veri değişti, cache'i temizle
        _memoryCache.Remove(CacheKey);
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null) return;

        // Soft Delete: Veriyi silmiyoruz, bayrağı kaldırıyoruz.
        entity.IsDeleted = true;
        _repository.Update(entity);
        await _unitOfWork.CommitAsync();

        // Veri değişti, cache'i temizle
        _memoryCache.Remove(CacheKey);
    }
}
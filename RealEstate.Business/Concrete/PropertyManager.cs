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

    public async Task<List<Property>> GetAllAsync()
    {
        // Veritabanındaki tüm evleri al
        var allProperties = await _propertyRepo.GetAllAsync();

        // Sadece IsDeleted = false (Silinmemiş) olanları filtrele
        return allProperties.Where(p => p.IsDeleted == false).ToList();
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
}

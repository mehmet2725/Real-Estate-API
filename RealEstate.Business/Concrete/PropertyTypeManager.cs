using AutoMapper;
using RealEstate.Business.Abstract;
using RealEstate.Business.Dtos.PropertyTypeDtos;
using RealEstate.Data.Abstract;
using RealEstate.Entity.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstate.Business.Concrete;

public class PropertyTypeManager : IPropertyTypeService
{
    private readonly IGenericRepository<PropertyType> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PropertyTypeManager(IGenericRepository<PropertyType> repository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<PropertyTypeDto>> GetAllAsync()
    {
        // IsDeleted olmayanları getir
        var values = await _repository.GetAllAsync();
        // Repository'de Where metodumuz var ama GetAllAsync list döndüğü için bellek içi filtreleme yapıyoruz şimdilik.
        // İdealde Repository'e GetListByFilter eklenebilir ama şu an bu yeterli.
        var activeValues = values.Where(x => !x.IsDeleted).ToList();

        return _mapper.Map<List<PropertyTypeDto>>(activeValues);
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
    }

    public async Task UpdateAsync(PropertyTypeUpdateDto updateDto)
    {
        var entity = await _repository.GetByIdAsync(updateDto.Id);
        if (entity == null) return;

        // updateDto verilerini entity üzerine yaz
        _mapper.Map(updateDto, entity);

        _repository.Update(entity);
        await _unitOfWork.CommitAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null) return;

        // Soft Delete: Veriyi silmiyoruz, bayrağı kaldırıyoruz.
        entity.IsDeleted = true;
        _repository.Update(entity);
        await _unitOfWork.CommitAsync();
    }
}

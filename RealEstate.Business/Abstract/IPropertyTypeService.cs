using RealEstate.Business.Dtos.PropertyTypeDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealEstate.Business.Abstract;

public interface IPropertyTypeService
{
    Task<List<PropertyTypeDto>> GetAllAsync();
    Task<PropertyTypeDto> GetByIdAsync(int id);
    Task AddAsync(PropertyTypeCreateDto createDto);
    Task UpdateAsync(PropertyTypeUpdateDto updateDto);
    Task DeleteAsync(int id);
}

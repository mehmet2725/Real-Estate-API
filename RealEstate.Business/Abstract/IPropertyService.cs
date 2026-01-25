using System.Collections.Generic;
using System.Threading.Tasks;
using RealEstate.Business.Dtos.PropertyDtos;
using RealEstate.Entity.Concrete;

namespace RealEstate.Business.Abstract;

public interface IPropertyService
{
    // Add advert
    // We are returning to the property.
    Task<Property> AddAsync(PropertyCreateDto createDto, int AgentId);

    // all advert list
    // İleride buraya PropertyListDto gelecek ama şimdilik Entity dönelim, yapıyı kuralım.
    Task<List<Property>> GetAllAsync();
}

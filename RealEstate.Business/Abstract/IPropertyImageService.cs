using RealEstate.Business.Dtos.PropertyImageDtos;
using System.Threading.Tasks;

namespace RealEstate.Business.Abstract;

public interface IPropertyImageService
{
    Task AddAsync(PropertyImageCreateDto propertyImageCreateDto);
}

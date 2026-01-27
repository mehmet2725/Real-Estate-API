using RealEstate.Business.Abstract;
using RealEstate.Business.Dtos.PropertyImageDtos;
using RealEstate.Data.Abstract;
using RealEstate.Entity.Concrete;
using System;
using System.IO;
using System.Threading.Tasks;

namespace RealEstate.Business.Concrete;

public class PropertyImageManager : IPropertyImageService
{
    private readonly IGenericRepository<PropertyImage> _propertyImageRepo;
    private readonly IUnitOfWork _unitOfWork;

    public PropertyImageManager(IGenericRepository<PropertyImage> propertyImageRepo, IUnitOfWork unitOfWork)
    {
        _propertyImageRepo = propertyImageRepo;
        _unitOfWork = unitOfWork;
    }
    public async Task AddAsync(PropertyImageCreateDto dto)
    {
        // Dosya boş mu?
        if(dto.File == null || dto.File.Length == 0)
        {
            return;
        }

        // dosya ismini benzersiz yapalım
        var extension = Path.GetExtension(dto.File.FileName);
        var newFileName = Guid.NewGuid() + extension;

        // Kaydedilecek klasörü belirle
        var location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

        // klasör yoksa oluştur
        if(!Directory.Exists(location))
        {
            Directory.CreateDirectory(location);
        }

        // Dosyayı fiziksel olarak diske kaydet
        var fullPath = Path.Combine(location, newFileName);

        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await dto.File.CopyToAsync(stream);
        }

        // Veritabanı nesnesini oluştur
        var PropertyImage = new PropertyImage
        {
            PropertyId = dto.PropertyId,
            ImageUrl = "/images/" + newFileName,
            Isprimary = dto.IsPrimary,
            DisplayOrder = 0
        };

        // kaydet
        await _propertyImageRepo.AddAsync(PropertyImage);
        await _unitOfWork.CommitAsync();
    }
}

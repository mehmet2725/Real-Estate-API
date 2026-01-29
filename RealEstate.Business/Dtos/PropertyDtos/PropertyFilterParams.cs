using System;

namespace RealEstate.Business.Dtos.PropertyDtos;

public class PropertyFilterParams
{
    // Arama Kriterleri
    public string? City { get; set; }
    public string? District { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public int? MinRooms { get; set; }
    public int? MaxRooms { get; set; }
    public int? PropertyTypeId { get; set; }
    public string? SearchKeyword { get; set; } // Başlık veya açıklama içinde arama

    // Sayfalama
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    // Sıralama
    public string SortBy { get; set; } = "createdAt"; // price, area, rooms
    public string SortOrder { get; set; } = "desc";   // asc, desc
}

using System;
using System.ComponentModel.DataAnnotations;  // for simple validation

namespace RealEstate.Business.Dtos.PropertyDtos;

public class PropertyCreateDto
{
    // Basic Informations
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;

    // Physical characteristics
    public int Rooms { get; set; }
    public int? Bathrooms { get; set; }
    public decimal Area { get; set; } // m2
    public int Floor { get; set; }
    public int? TotalFloors { get; set; }
    public int YearBuilt { get; set; }

    // Relationships
    public int PropertyId { get; set; }

    // Emlakçıyı (AgentId) elle almayacağız, token'dan (giriş yapan kullanıcıdan) çekeceğiz.
    // O yüzden buraya koymuyoruz.
}

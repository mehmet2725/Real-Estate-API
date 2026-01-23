using System;
using RealEstate.Entity.Abstract;

namespace RealEstate.Entity.Concrete;

public class Property : BaseClass
{
    // Basic Information
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;

    // Physical Characteristics
    public int Rooms { get; set; }
    public int? Bathrooms { get; set; }
    public decimal Area { get; set; }
    public int Floor { get; set; }
    public int? TotalFloors { get; set; }
    public int YearBuilt { get; set; }

    // Status
    public string Status { get; set; } = string.Empty;

    // PropertyType Relationship
    public int PropertyTypeId { get; set; }
    public PropertyType PropertyType { get; set; } = null!;// navigaton Prop

    // An advertisement can have multiple images
    public ICollection<PropertyImage> Images { get; set; } = new List<PropertyImage>();
    // An advertisement posting may receive multiple inquiry
    public ICollection<Inquiry> Inquiries { get; set; } = new List<Inquiry>();

    // Agent Relationship
    public int AgentId { get; set; }
    public AppUser Agent { get; set; } = null!;

}

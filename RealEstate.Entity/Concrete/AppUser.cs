using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace RealEstate.Entity.Concrete;

public class AppUser : IdentityUser<int>
{
    // Personel Info
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? ProfilePicture { get; set; }

    // Agent Info
    // Not every user is a real estate agent.
    public bool IsAgent { get; set; } = false;
    public string? AgencyName { get; set; }
    public string? LicenseNumber { get; set; }

    // Relationships

    // A real estate agent would have their own listings.
    public ICollection<Property> Properties { get; set; } = new List<Property>();

    // There are messages sent by a user.
    public ICollection<Inquiry> Inquiries { get; set; } = new List<Inquiry>();

    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenEndDate { get; set; }
}

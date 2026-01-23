using RealEstate.Entity.Abstract;

namespace RealEstate.Entity.Concrete;

public class Inquiry : BaseClass
{
    // Contact Info
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string Message { get; set; } = string.Empty;


    public string Status { get; set; } = "Yeni";

    // for which advertisement
    public int PropertyId { get; set; }
    public Property Property { get; set; } = null!;

    // Which user posted this?
    public int? UserId { get; set; }
    public AppUser? User { get; set; }
}

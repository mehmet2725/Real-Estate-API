using RealEstate.Entity.Abstract;

namespace RealEstate.Entity.Concrete;

public class PropertyImage : BaseClass
{
    public String ImageUrl { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public bool Isprimary { get; set; }
    public int PropertyId { get; set; }
    public Property Property { get; set; } = null!;
}

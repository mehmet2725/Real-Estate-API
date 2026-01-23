using System.Collections.Generic;
using RealEstate.Entity.Abstract;

namespace RealEstate.Entity.Concrete;

public class PropertyType : BaseClass
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public ICollection<Property> Properties { get; set; } = new List<Property>();
}

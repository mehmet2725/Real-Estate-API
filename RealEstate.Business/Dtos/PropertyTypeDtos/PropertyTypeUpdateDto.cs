using System.ComponentModel.DataAnnotations;

namespace RealEstate.Business.Dtos.PropertyTypeDtos;

public class PropertyTypeUpdateDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Emlak Tipi adı zorunludur")]
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}

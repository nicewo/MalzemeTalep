using System.ComponentModel.DataAnnotations;

namespace MalzemeTalep.Models;

public class Material
{
    public int Id { get; set; }
    [MaxLength(120)] public string Brand { get; set; } = "";
    [MaxLength(120)] public string Category { get; set; } = "";
    [Required, MaxLength(160)] public string ModelCode { get; set; } = "";
    [Required, MaxLength(500)] public string Description { get; set; } = "";
    [MaxLength(1000)] public string TechnicalSpecs { get; set; } = "";
    [MaxLength(40)] public string Unit { get; set; } = "ADET";
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

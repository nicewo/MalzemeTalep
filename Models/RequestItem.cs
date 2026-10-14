using System.ComponentModel.DataAnnotations;

namespace MalzemeTalep.Models;

public class RequestItem
{
    public int Id { get; set; }
    public int RequestId { get; set; }
    public MaterialRequest? Request { get; set; }
    public int? MaterialId { get; set; }
    public Material? Material { get; set; }
    [Required, MaxLength(160)] public string BrandSnapshot { get; set; } = "";
    [Required, MaxLength(160)] public string ModelCodeSnapshot { get; set; } = "";
    [Required, MaxLength(500)] public string DescriptionSnapshot { get; set; } = "";
    [MaxLength(1000)] public string SpecsSnapshot { get; set; } = "";
    [Range(0.001, 100000000)] public decimal Quantity { get; set; } = 1;
    [Required, MaxLength(40)] public string UnitSnapshot { get; set; } = "ADET";
}

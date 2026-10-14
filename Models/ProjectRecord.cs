using System.ComponentModel.DataAnnotations;

namespace MalzemeTalep.Models;

public class ProjectRecord
{
    public int Id { get; set; }
    [Required, MaxLength(200)] public string Name { get; set; } = "";
    [MaxLength(200)] public string Customer { get; set; } = "";
    [MaxLength(1000)] public string Notes { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

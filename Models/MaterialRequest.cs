using System.ComponentModel.DataAnnotations;

namespace MalzemeTalep.Models;

public class MaterialRequest
{
    public int Id { get; set; }
    [Required, MaxLength(200)] public string Title { get; set; } = "";
    public int? ProjectRecordId { get; set; }
    public ProjectRecord? ProjectRecord { get; set; }
    public DateTime RequestDate { get; set; } = DateTime.Today;
    [MaxLength(1000)] public string Notes { get; set; } = "";
    public List<RequestItem> Items { get; set; } = [];
}

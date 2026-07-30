using System.ComponentModel.DataAnnotations;

namespace BlazorCarRepairsApp.Dto.Technicians;

public class TechEditDto
{
    public Guid Id { get; set; }
    
    [Required]
    [MaxLength(30)]
    public string Specialty { get; set; } = string.Empty;
    
    public Guid? UserId { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
}
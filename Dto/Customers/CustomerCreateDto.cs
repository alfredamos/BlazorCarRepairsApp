using System.ComponentModel.DataAnnotations;

namespace BlazorCarRepairsApp.Dto.Customers;

public class CustomerCreateDto
{
    [Required]
    [MaxLength(1000)]
    public string Address { get; set; } = string.Empty;
    
    public bool Active { get; set; } = true;

    [Required]
    [MaxLength(1000)]
    public string Notes { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
    
    public Guid UserId { get; set; } 
}
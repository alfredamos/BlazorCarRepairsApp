using System.ComponentModel.DataAnnotations;

namespace BlazorCarRepairsApp.Dto.Customers;

public class CustomerEditDto
{
    [Required]
    public Guid Id { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Address { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(1000)]
    public string Notes { get; set; } = string.Empty;
    
    public bool Active { get; set; } = true;
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
    
    public Guid UserId { get; set; } 
}
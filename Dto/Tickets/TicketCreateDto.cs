using System.ComponentModel.DataAnnotations;

namespace BlazorCarRepairsApp.Dto.Tickets;

public class TicketCreateDto
{
    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;
    
    public Guid CustomerId { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }

    
}
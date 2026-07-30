namespace BlazorCarRepairsApp.Dto.Tickets;

public class TicketResponse
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public Guid CustomerId { get; set; }

    public string? Name { get; set; } = string.Empty;
    
    public string? Address { get; set; } = string.Empty;
    
    public string? Email { get; set; } = string.Empty;
    
    public string? Phone { get; set; } = string.Empty;

    public string? Gender { get; set; } = string.Empty;

    public string? Image { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }

}
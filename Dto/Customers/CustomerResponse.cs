

namespace BlazorCarRepairsApp.Dto.Customers;

public class CustomerResponse
{
    public Guid Id { get; set; }
    public string? Address { get; set; } = string.Empty;
    public string? Email { get; set; } = string.Empty;
    public string? Gender { get; set; } = string.Empty;
    public string? Name { get; set; } = string.Empty;
    public string? Phone { get; set; } = string.Empty;
    public string? Image { get; set; } = string.Empty;
    
    public string? Notes { get; set; } = string.Empty;
    public bool Active { get; set; } = true;
    public DateOnly?  Birthdate { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }

    public Guid UserId { get; set; }

}
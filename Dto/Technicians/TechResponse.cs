namespace BlazorCarRepairsApp.Dto.Technicians;

public class TechResponse
{
    public Guid Id { get; set; }
    
    public string Specialty { get; set; } = string.Empty;

    public Guid UserId { get; set; }
    
    public DateOnly? Birthdate { get; set; }

    public string? Name { get; set; } = string.Empty;
    
    public string? Email { get; set; } = string.Empty;
    
    public string? Phone { get; set; } = string.Empty;
    
    public string? Image { get; set; } = string.Empty;
    
    public string? Gender { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
}
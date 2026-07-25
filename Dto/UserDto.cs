namespace BlazorCarRepairsApp.Dto;

public class UserDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string? Image { get; set; } = string.Empty;
    public string? Phone { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty; 
}
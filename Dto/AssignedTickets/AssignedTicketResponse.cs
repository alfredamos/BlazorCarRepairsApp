using BlazorCarRepairsApp.Models;

namespace BlazorCarRepairsApp.Dto.AssignedTickets;

public class AssignedTicketResponse
{
    public Guid TicketId { get; set; }

    public Guid TechnicianId { get; set; }

    public DateTime AssignAt { get; set; }

    public string AssignBy { get; set; } = string.Empty;

    public bool Completed { get; set; }

    public Status Status { get; set; }

    public string TicketTitle { get; set; } = string.Empty;
    
    public string TicketDescription { get; set; } = string.Empty;
    
    public string? CustomerName { get; set; } = string.Empty;
    
    public string? CustomerAddress { get; set; } = string.Empty;
    
    public string? CustomerEmail { get; set; } = string.Empty;
    
    public string? CustomerPhone { get; set; } = string.Empty;
    
    public string? CustomerImage { get; set; } = string.Empty;
    
    public string? TechnicianName { get; set; } = string.Empty;
    
    public string? TechnicianEmail { get; set; } = string.Empty;
    
    public string? TechnicianPhone { get; set; } = string.Empty;
    
    public string? TechnicianImage { get; set; } = string.Empty;
    
    public string TechnicianSpecialty { get; set; } = string.Empty;
}
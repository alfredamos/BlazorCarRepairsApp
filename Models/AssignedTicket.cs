using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorCarRepairsApp.Models;

public class AssignedTicket
{
    public Guid TicketId { get; set; }

    public Ticket Ticket { get; set; } = null!;
    
    public Guid TechnicianId { get; set; }

    public Technician Technician { get; set; } = null!;

    [EnumDataType(typeof(Status))] public Status Status { get; set; } = Status.Open;

    public bool Completed { get; set; } = false;

    public DateTime AssignAt { get; set; }

    [MaxLength(100)]
    [Required]
    public string AssignBy { get; set; } = string.Empty;
}
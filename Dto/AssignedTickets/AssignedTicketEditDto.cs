using System.ComponentModel.DataAnnotations;
using BlazorCarRepairsApp.Models;

namespace BlazorCarRepairsApp.Dto.AssignedTickets;

public class AssignedTicketEditDto
{
    public Guid TicketId { get; set; }

    public Guid TechnicianId { get; set; }

    public bool Completed { get; set; }
    
    public Status Status { get; set; }

}
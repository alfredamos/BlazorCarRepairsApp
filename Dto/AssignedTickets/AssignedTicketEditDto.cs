using System.ComponentModel.DataAnnotations;

namespace BlazorCarRepairsApp.Dto.AssignedTickets;

public class AssignedTicketEditDto
{
    public Guid TicketId { get; set; }

    public Guid TechnicianId { get; set; }

    public bool Completed { get; set; }

}
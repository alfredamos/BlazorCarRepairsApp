using System.ComponentModel.DataAnnotations;

namespace BlazorCarRepairsApp.Dto.AssignedTickets;

public class AssignedTicketCreateDto
{
    public Guid TicketId { get; set; }

    public Guid TechnicianId { get; set; }

}
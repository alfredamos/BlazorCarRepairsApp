using BlazorCarRepairsApp.Dto.AssignedTickets;
using BlazorCarRepairsApp.Models;

namespace BlazorCarRepairsApp.Mappers;

public static class AssignedTicketMapper
{
    public static AssignedTicket MapAssignedTicketCreateDtoToAssignedTicket(AssignedTicketCreateDto dto, string assignBy)
    {
        return new AssignedTicket
        {
            TicketId = dto.TicketId,
            TechnicianId = dto.TechnicianId,
            AssignBy = assignBy,
            Status = Status.Open,
            Completed = false,

        };
    }
    
    public static AssignedTicketResponse MapToAssignedTicketResponse(AssignedTicket assignedTicket)
    {
        return new AssignedTicketResponse
        {
            TicketId = assignedTicket.TicketId,
            TicketTitle = assignedTicket.Ticket.Title,
            TicketDescription = assignedTicket.Ticket.Description,
            TechnicianId = assignedTicket.TechnicianId,
            AssignAt = assignedTicket.AssignAt,
            AssignBy = assignedTicket.AssignBy,
            Status = assignedTicket.Status,
            Completed = assignedTicket.Completed,
            CustomerName = assignedTicket.Ticket.Customer.User?.Name,
            CustomerEmail = assignedTicket.Ticket.Customer.User?.Email,
            CustomerPhone = assignedTicket.Ticket.Customer.User?.PhoneNumber,
            CustomerAddress = assignedTicket.Ticket.Customer.Address,
            CustomerImage = assignedTicket.Ticket.Customer.User?.ImagePath,
            TechnicianName = assignedTicket.Technician.User?.Name,
            TechnicianEmail = assignedTicket.Technician.User?.Email,
            TechnicianPhone = assignedTicket.Technician.User?.PhoneNumber,
            TechnicianImage = assignedTicket.Technician.User?.ImagePath,
            TechnicianSpecialty = assignedTicket.Technician.Specialty,
        };
    }
}
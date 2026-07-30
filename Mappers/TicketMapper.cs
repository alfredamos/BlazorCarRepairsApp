using BlazorCarRepairsApp.Dto.Technicians;
using BlazorCarRepairsApp.Dto.Tickets;
using BlazorCarRepairsApp.Models;

namespace BlazorCarRepairsApp.Mappers;

public static class TicketMapper
{
    public static Ticket MapTicketCreateDtoToTicket(TicketCreateDto dto)
    {
        return new Ticket
        {
            Title = dto.Title,
            Description = dto.Description,
            CustomerId = dto.CustomerId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };
    }
    public static Ticket MapTicketEditDtoToTicket(TicketEditDto dto, Ticket ticket)
    {
        ticket.Id = dto.Id;
        ticket.Title = dto.Title;
        ticket.Description = dto.Description;
        ticket.CustomerId = dto.CustomerId;
        ticket.CreatedAt = dto.CreatedAt;
        ticket.UpdatedAt = DateTime.UtcNow;
        
        return ticket;
    }

    public static TicketResponse MapToTicketResponse(Ticket ticket)
    {
        return new TicketResponse
        {
            Id = ticket.Id,
            Title = ticket.Title,
            Description = ticket.Description,
            Address = ticket.Customer.Address,
            Name = ticket.Customer.User?.Name,
            Email = ticket.Customer.User?.Email,
            Phone = ticket.Customer.User?.PhoneNumber,
            Image = ticket.Customer.User?.ImagePath,
            Gender = ticket.Customer.User?.Gender,
            CustomerId = ticket.CustomerId,
            CreatedAt = ticket.CreatedAt,
            UpdatedAt = ticket.UpdatedAt,
        };
    }
}
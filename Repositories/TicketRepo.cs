using System.Net;
using BlazorCarRepairsApp.Contracts;
using BlazorCarRepairsApp.Data;
using BlazorCarRepairsApp.Dto;
using BlazorCarRepairsApp.Dto.Tickets;
using BlazorCarRepairsApp.Exceptions;
using BlazorCarRepairsApp.Mappers;
using BlazorCarRepairsApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorCarRepairsApp.Repositories;

public class TicketRepo(ApplicationDbContext context) : ITicketRepo
{
    public async Task<ResponseMessage> CreateTicket(TicketCreateDto ticketDto)
    {
        //----> Map ticket-create-dto to ticket.
        var ticket = TicketMapper.MapTicketCreateDtoToTicket(ticketDto);
        
        //----> Insert the new ticket into db.
        await context.Tickets.AddAsync(ticket);
        await context.SaveChangesAsync();
        
        //----> Send back response.
        return new ResponseMessage
        {
            Message = "Ticket created successfully!",
            Status = "Success",
            StatusCode = HttpStatusCode.Created
        };

    }

    public async Task<ResponseMessage> DeleteTicketById(Guid id)
    {
        //----> Check for existence of ticket.
        var ticket = await GetOneTicket(id);
        
        //----> Delete the ticket with the giving id.
        context.Tickets.Remove(ticket);
        await context.SaveChangesAsync();
        
        //----> Send back response.
        return new ResponseMessage
        {
            Message = "Ticket deleted successfully!",
            Status = "Success",
            StatusCode = HttpStatusCode.OK
        };
    }

    public async Task<ResponseMessage> EditTicketById(Guid id, TicketEditDto ticketDto)
    {
        //----> Check for existence of ticket.
        var ticket = await GetOneTicket(id);
        
        //----> Map ticket-edit-dto to ticket.
        ticket = TicketMapper.MapTicketEditDtoToTicket(ticketDto, ticket, ticket.CreatedAt);
        
        //----> Update the ticket details in db.
        context.Tickets.Update(ticket);
        await context.SaveChangesAsync();
        
        //----> Send back response.
        return new ResponseMessage
        {
            Message = "Ticket edited successfully!",
            Status = "Success",
            StatusCode = HttpStatusCode.OK
        };
    }

    public async Task<TicketResponse> GetTicketById(Guid id)
    {
        //----> Fetch the ticket with the giving id.
        var ticket = await GetOneTicket(id);
        
        //----> Send back response.
        return TicketMapper.MapToTicketResponse(ticket);
    }

    public async Task<List<TicketResponse>> GetTickets(string? searchItem)
    {
        var query = context.Tickets
            .Include(tk => tk.Customer)
            .ThenInclude(cst => cst.User)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchItem))
        {
            var search = searchItem.Trim().ToLower();

            query = query.Where(cst => 
                (cst.Title.ToLower().Contains(search)) ||
                (cst.Description.ToLower().Contains(search)) ||
                (cst.Customer.User != null && (
                    (cst.Customer.User.Name.ToLower().Contains(search)) ||
                    (cst.Customer.User.Email != null && cst.Customer.User.Email.ToLower().Contains(search)) ||
                    (cst.Customer.User.PhoneNumber != null && cst.Customer.User.PhoneNumber.Contains(search)) ||
                    (cst.Customer.User.Gender.ToLower().Contains(search))
                ))
            );
        }

        var tickets = await query.ToListAsync();
    
        return [.. tickets.Select(TicketMapper.MapToTicketResponse)];
    }


    public async Task<List<TicketResponse>> GetTicketsByCustomerId(Guid customerId, string? searchItem = "")
    {
        //----> Fetch the ticket with the giving customer id.
        var tickets =  (await GetTickets(searchItem)).Where(tk => tk.CustomerId.Equals(customerId)).ToList();
        
        //----> Send back response.
        return tickets;
    }

    private async Task<Ticket> GetOneTicket(Guid id)
    {
        //----> Fetch the ticket with the giving id.
        var ticket = await context.Tickets.Include(tk => tk.Customer).ThenInclude(tk => tk.User).AsNoTracking()
            .FirstOrDefaultAsync(tk => tk.Id.Equals(id));

        //----> Check for null ticket and send back response.
        return ticket ?? throw new CustomException("Ticket not found", HttpStatusCode.NotFound);
    }
}
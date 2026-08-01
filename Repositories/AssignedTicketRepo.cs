using System.Net;
using BlazorCarRepairsApp.Contracts;
using BlazorCarRepairsApp.Data;
using BlazorCarRepairsApp.Dto;
using BlazorCarRepairsApp.Dto.AssignedTickets;
using BlazorCarRepairsApp.Exceptions;
using BlazorCarRepairsApp.Mappers;
using BlazorCarRepairsApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorCarRepairsApp.Repositories;

public class AssignedTicketRepo(ApplicationDbContext context, IUserRepo userRepo) : IAssignedTicketRepo
{
    public async Task<ResponseMessage> CreateAssignedTicket(AssignedTicketCreateDto assignedTicketDto)
    {
        //----> Get the name of admin user as assignor
        var assignBy = (await userRepo.GetCurrentUser()).Name;
        if (assignBy is null) throw new CustomException("You must login!", HttpStatusCode.Unauthorized);
        
        //----> Map assigned-ticket-create-dto to Assigned-ticket.
        var assignedTicket =
            AssignedTicketMapper.MapAssignedTicketCreateDtoToAssignedTicket(assignedTicketDto, assignBy);
        
        //----> Insert the assigned-ticket in db.
        await context.AssignedTickets.AddAsync(assignedTicket);
        await context.SaveChangesAsync();
        
        //----> Send back response
        return new ResponseMessage
        {
            Message = "AssignedTicket created successfully",
            Status = "Success",
            StatusCode = HttpStatusCode.Created
        };
    }

    public async Task<ResponseMessage> ChangeAssignedTicketStatus(Guid technicianId, Guid ticketId)
    {
        //----> Fetch the assigned-ticket with the giving ids.
        var  assignedTicket = await GetOneAssignedTicket(technicianId, ticketId);
        
        //----> Get the ticket status.
        assignedTicket.Completed = !assignedTicket.Completed;
        assignedTicket.Status = assignedTicket.Completed ? Status.Closed : Status.Open;
        context.AssignedTickets.Update(assignedTicket);
        await context.SaveChangesAsync();

        return new ResponseMessage
        {
            Message = "AssignedTicket updated successfully",
            Status = "Success",
            StatusCode = HttpStatusCode.OK
        };
    }

    public async Task<ResponseMessage> DeleteAssignedTicketById(Guid technicianId, Guid ticketId)
    {
        //----> Get the assigned-ticket with the giving ids.
        var assignedTicket = await GetOneAssignedTicket(technicianId, ticketId);
        
        //----> Delete the assigned-ticket with the giving ids.
        context.AssignedTickets.Remove(assignedTicket);
        await context.SaveChangesAsync();

        //----> Send back response
        return new ResponseMessage
        {
            Message = "AssignedTicket deleted successfully",
            Status = "Success",
            StatusCode = HttpStatusCode.OK
        };
    }

    public async Task<ResponseMessage> EditAssignedTicketById(Guid technicianId, Guid ticketId, AssignedTicketEditDto tkRequest)
    {
        //----> Get the assigned-ticket with the giving ids.
        var assignedTicket = await GetOneAssignedTicket(technicianId, ticketId);
        
        //----> Check the status.
        Console.WriteLine($"In edit-assigned-ticket, completed : {tkRequest.Completed}");
        Console.WriteLine($"In edit-assigned-ticket, completed : {tkRequest.Completed}");
        assignedTicket.Status = tkRequest.Status;
        assignedTicket.Completed  = tkRequest.Status == Status.Closed;
        
        //----> Update the assigned-ticket details in db.
        context.AssignedTickets.Update(assignedTicket);
        await context.SaveChangesAsync();
        
        //----> Send back response.
        return new ResponseMessage
        {
            Message = "AssignedTicket edited successfully",
            Status = "Success",
            StatusCode = HttpStatusCode.OK
        };
    }

        public async Task<List<AssignedTicketResponse>> GetAllAssignedTickets(string? searchItem = "")
    {
        // Normalize search term once
        var search = searchItem?.Trim().ToLower() ?? string.Empty;

        // Set up query with necessary includes
        var query = context.AssignedTickets
            .Include(atk => atk.Ticket)
                .ThenInclude(t => t.Customer)
                    .ThenInclude(c => c.User)
            .Include(atk => atk.Technician)
                .ThenInclude(tech => tech.User)
            .AsQueryable();

        // Apply search filter if searchItem is provided
        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(atk =>
                // Ticket properties
                (!string.IsNullOrEmpty(atk.Ticket.Title) && atk.Ticket.Title.ToLower().Contains(search)) ||
                (!string.IsNullOrEmpty(atk.Ticket.Description) && atk.Ticket.Description.ToLower().Contains(search)) ||
                
                // Customer User properties
                (atk.Ticket.Customer.User != null && (
                    (!string.IsNullOrEmpty(atk.Ticket.Customer.User.Name) && atk.Ticket.Customer.User.Name.ToLower().Contains(search)) ||
                    (!string.IsNullOrEmpty(atk.Ticket.Customer.User.Email) && atk.Ticket.Customer.User.Email.ToLower().Contains(search)) ||
                    (!string.IsNullOrEmpty(atk.Ticket.Customer.User.PhoneNumber) && atk.Ticket.Customer.User.PhoneNumber.Contains(search)) ||
                    (!string.IsNullOrEmpty(atk.Ticket.Customer.User.Gender) && atk.Ticket.Customer.User.Gender.ToLower().Contains(search))
                )) ||
                
                // Technician properties
                (!string.IsNullOrEmpty(atk.Technician.Specialty) && atk.Technician.Specialty.ToLower().Contains(search)) ||
                
                // Technician User properties
                (atk.Technician.User != null && (
                    (!string.IsNullOrEmpty(atk.Technician.User.Name) && atk.Technician.User.Name.ToLower().Contains(search)) ||
                    (!string.IsNullOrEmpty(atk.Technician.User.Email) && atk.Technician.User.Email.ToLower().Contains(search)) ||
                    (!string.IsNullOrEmpty(atk.Technician.User.PhoneNumber) && atk.Technician.User.PhoneNumber.Contains(search)) ||
                    (!string.IsNullOrEmpty(atk.Technician.User.Gender) && atk.Technician.User.Gender.ToLower().Contains(search))
                ))
            );
        }

        // Execute query and map to response model (handles both filtered and unfiltered cases)
        var assignedTickets = await query.ToListAsync();

        return [.. assignedTickets.Select(AssignedTicketMapper.MapToAssignedTicketResponse)];
    }


    public async Task<AssignedTicketResponse> GetAssignedTicketById(Guid technicianId, Guid ticketId)
    {
       //----> Fetch the assigned ticket with giving ids.
       var ticket = await GetOneAssignedTicket(technicianId, ticketId);
       
       //----> Send back response.
       return AssignedTicketMapper.MapToAssignedTicketResponse(ticket);
    }

    public async Task<List<AssignedTicketResponse>> GetAssignedTicketsByTechnicianId(Guid technicianId, string? searchItem = "")
    {
        //----> Get assigned-tickets by tech-id.
        var tickets = (await GetAllAssignedTickets(searchItem)).Where(tk => tk.TechnicianId.Equals(technicianId)).ToList();
        
        //----> Send back response
        return tickets;
    }

    public async Task<List<AssignedTicketResponse>> GetAssignedTicketsByTicketId(Guid ticketId, string? searchItem = "")
    {
        //----> Get assigned-tickets by ticket-id.
        var tickets = (await GetAllAssignedTickets(searchItem)).Where(tk => tk.TicketId.Equals(ticketId)).ToList();
        
        //----> Send back response
        return tickets;
    }

    public async Task<List<AssignedTicketResponse>> GetCompletedAssignedTickets(string? searchItem = "")
    {
        //----> Fetch all completed tickets.
        var tickets = (await GetAllAssignedTickets(searchItem)).Where(tk => tk.Completed).ToList();
        
        //----> Send back response.
        return tickets;
    }

    public async Task<List<AssignedTicketResponse>> GetUncompletedAssignedTickets(string? searchItem = "")
    {
        //----> Fetch all uncompleted tickets.
        var tickets = (await GetAllAssignedTickets(searchItem)).Where(tk => !tk.Completed).ToList();
        
        //----> Send back response.
        return tickets;
    }

    private async Task<AssignedTicket> GetOneAssignedTicket(Guid technicianId, Guid ticketId)
    {
        //----> Fetch the assigned ticket with the giving ids.
        var ticket = await context.AssignedTickets
            .Include(tk => tk.Technician.User)
            .Include(tk => tk.Ticket.Customer.User)
            .AsNoTracking()
            .FirstOrDefaultAsync(tk => tk.TechnicianId == technicianId && tk.TicketId == ticketId);
        
        //----> Send back response.
        return ticket ??  throw new CustomException("Ticket not found!", HttpStatusCode.NotFound);
    }

    
}
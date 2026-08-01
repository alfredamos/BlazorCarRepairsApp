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

    public async Task<List<AssignedTicketResponse>> GetAllAssignedTickets()
    {
        //----> Fetch all assigned-tickets.
        var assignedTickets = await context.AssignedTickets
            .Include(tk => tk.Technician.User)
            .Include(tk => tk.Ticket.Customer.User)
            .AsNoTracking().ToListAsync();
        
        //----> Send back response.
        return [.. assignedTickets.Select(AssignedTicketMapper.MapToAssignedTicketResponse)];
    }

    public async Task<AssignedTicketResponse> GetAssignedTicketById(Guid technicianId, Guid ticketId)
    {
        //----> Get assigned-tickets by ticket-id.
       var assignedTicket = await GetOneAssignedTicket(technicianId, ticketId);
       
       //----> Send back response.
       return AssignedTicketMapper.MapToAssignedTicketResponse(assignedTicket);
    }

    public async Task<List<AssignedTicketResponse>> GetAssignedTicketsByTechnicianId(Guid technicianId)
    {
        //----> Get assigned-tickets by tech-id.
        var tickets = (await GetAllAssignedTickets()).Where(tk => tk.TechnicianId.Equals(technicianId)).ToList();
        
        //----> Send back response
        return tickets;
    }

    public async Task<List<AssignedTicketResponse>> GetAssignedTicketsByTicketId(Guid ticketId)
    {
        //----> Get assigned-tickets by ticket-id.
        var tickets = (await GetAllAssignedTickets()).Where(tk => tk.TicketId.Equals(ticketId)).ToList();
        
        //----> Send back response
        return tickets;
    }

    public async Task<List<AssignedTicketResponse>> GetCompletedAssignedTickets()
    {
        //----> Fetch all completed tickets.
        var tickets = (await GetAllAssignedTickets()).Where(tk => tk.Completed).ToList();
        
        //----> Send back response.
        return tickets;
    }

    public async Task<List<AssignedTicketResponse>> GetUncompletedAssignedTickets()
    {
        //----> Fetch all uncompleted tickets.
        var tickets = (await GetAllAssignedTickets()).Where(tk => !tk.Completed).ToList();
        
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
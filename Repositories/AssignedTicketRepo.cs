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

public class AssignedTicketRepo(ApplicationDbContext context,IHttpContextAccessor httpContextAccessor , IUserRepo userRepo) : IAssignedTicketRepo
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task<ResponseMessage> CreateAssignedTicket(AssignedTicketCreateDto assignedTicketDto)
    {
        //----> Get the name of admin user as assignor
        var assignBy = (await GetCurrentUser()).Name;
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

    public async Task<ResponseMessage> ChangeAssignedTicketStatus(Guid ticketId, Guid technicianId)
    {
        //----> Fetch the assigned-ticket with the giving ids.
        var  assignedTicket = await GetOneAssignedTicket(ticketId, technicianId);
        
        //----> Get the ticket status.
        assignedTicket.Completed = !assignedTicket.Completed;
        assignedTicket.Status = assignedTicket.Completed ? Status.Closed : Status.Open;
        await context.SaveChangesAsync();

        return new ResponseMessage
        {
            Message = "AssignedTicket updated successfully",
            Status = "Success",
            StatusCode = HttpStatusCode.OK
        };
    }

    public async Task<ResponseMessage> DeleteAssignedTicketById(Guid ticketId, Guid technicianId)
    {
        //----> Get the assigned-ticket with the giving ids.
        var assignedTicket = await GetOneAssignedTicket(ticketId, technicianId);
        
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

    public async Task<ResponseMessage> EditAssignedTicketById(Guid ticketId, Guid technicianId, AssignedTicketEditDto assignedTicketRequest)
    {
        //----> Get the assigned-ticket with the giving ids.
        var assignedTicket = await GetOneAssignedTicket(ticketId, technicianId);
        
        //----> Map assigned-ticket-edit-dto to assigned-ticket.
        assignedTicket = AssignedTicketMapper.MapAssignedTicketEditDtoToAssignedTicket(assignedTicketRequest, assignedTicket);
        
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

    public async Task<AssignedTicketResponse> GetAssignedTicketById(Guid ticketId, Guid technicianId)
    {
        //----> Get assigned-tickets by ticket-id.
       var assignedTicket = await GetOneAssignedTicket(ticketId, technicianId);
       
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

    private async Task<AssignedTicket> GetOneAssignedTicket(Guid ticketId, Guid technicianId)
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

    private  async Task<UserDto> GetCurrentUser()
    {
        //----> Get the HTTP context.
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext is null) throw new CustomException("You need to be logged in first", HttpStatusCode.Unauthorized);;
        var email = httpContext.User.Identity?.Name;
        if (email is null) throw new CustomException("You must login!", HttpStatusCode.Unauthorized);
        var user = await userRepo.GetCurrentUserByEmail(email);
        return user;

    }
}
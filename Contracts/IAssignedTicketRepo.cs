using BlazorCarRepairsApp.Dto;
using BlazorCarRepairsApp.Dto.AssignedTickets;

namespace BlazorCarRepairsApp.Contracts;

public interface IAssignedTicketRepo
{
    Task<ResponseMessage> CreateAssignedTicket(AssignedTicketCreateDto assignedTicketRequest);
    Task<ResponseMessage> ChangeAssignedTicketStatus(Guid technicianId, Guid ticketId);
    Task<ResponseMessage> DeleteAssignedTicketById(Guid technicianId, Guid ticketId);
    Task<ResponseMessage> EditAssignedTicketById(Guid technicianId, Guid ticketId, AssignedTicketEditDto assignedTicketRequest);
    Task<List<AssignedTicketResponse>> GetAllAssignedTickets(string? searchItem = "");
    Task<AssignedTicketResponse> GetAssignedTicketById(Guid technicianId, Guid ticketId);
    Task<List<AssignedTicketResponse>> GetAssignedTicketsByTechnicianId(Guid technicianId, string? searchItem = "");
    Task<List<AssignedTicketResponse>> GetAssignedTicketsByTicketId(Guid technicianId, string? searchItem = "");
    Task<List<AssignedTicketResponse>> GetCompletedAssignedTickets(string? searchItem = "");
    Task<List<AssignedTicketResponse>> GetUncompletedAssignedTickets(string? searchItem = "");
}


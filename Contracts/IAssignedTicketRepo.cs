using BlazorCarRepairsApp.Dto;
using BlazorCarRepairsApp.Dto.AssignedTickets;

namespace BlazorCarRepairsApp.Contracts;

public interface IAssignedTicketRepo
{
    Task<ResponseMessage> CreateAssignedTicket(AssignedTicketCreateDto assignedTicketRequest);
    Task<ResponseMessage> ChangeAssignedTicketStatus(Guid ticketId, Guid technicianId);
    Task<ResponseMessage> DeleteAssignedTicketById(Guid ticketId, Guid technicianId);
    Task<ResponseMessage> EditAssignedTicketById(Guid ticketId, Guid technicianId, AssignedTicketEditDto assignedTicketRequest);
    Task<List<AssignedTicketResponse>> GetAllAssignedTickets();
    Task<AssignedTicketResponse> GetAssignedTicketById(Guid ticketId, Guid technicianId);
    Task<List<AssignedTicketResponse>> GetAssignedTicketsByTechnicianId(Guid technicianId);
    Task<List<AssignedTicketResponse>> GetAssignedTicketsByTicketId(Guid technicianId);
    Task<List<AssignedTicketResponse>> GetCompletedAssignedTickets();
    Task<List<AssignedTicketResponse>> GetUncompletedAssignedTickets();
}


using BlazorCarRepairsApp.Dto;
using BlazorCarRepairsApp.Dto.AssignedTickets;

namespace BlazorCarRepairsApp.Contracts;

public interface IAssignedTicketRepo
{
    Task<ResponseMessage> CreateAssignedTicket(AssignedTicketCreateDto assignedTicketRequest);
    Task<ResponseMessage> ChangeAssignedTicketStatus(Guid technicianId, Guid ticketId);
    Task<ResponseMessage> DeleteAssignedTicketById(Guid technicianId, Guid ticketId);
    Task<ResponseMessage> EditAssignedTicketById(Guid technicianId, Guid ticketId, AssignedTicketEditDto assignedTicketRequest);
    Task<List<AssignedTicketResponse>> GetAllAssignedTickets();
    Task<AssignedTicketResponse> GetAssignedTicketById(Guid technicianId, Guid ticketId);
    Task<List<AssignedTicketResponse>> GetAssignedTicketsByTechnicianId(Guid technicianId);
    Task<List<AssignedTicketResponse>> GetAssignedTicketsByTicketId(Guid technicianId);
    Task<List<AssignedTicketResponse>> GetCompletedAssignedTickets();
    Task<List<AssignedTicketResponse>> GetUncompletedAssignedTickets();
}


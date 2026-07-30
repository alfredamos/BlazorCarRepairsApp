using BlazorCarRepairsApp.Dto;
using BlazorCarRepairsApp.Dto.Technicians;
using BlazorCarRepairsApp.Dto.Tickets;

namespace BlazorCarRepairsApp.Contracts;

public interface ITicketRepo
{
    Task<ResponseMessage> CreateTicket(TicketCreateDto ticket);
    Task<ResponseMessage> DeleteTicketById(Guid id);
    Task<ResponseMessage> EditTicketById(Guid id, TicketEditDto ticket);
    Task<TicketResponse> GetTicketById(Guid id);
    Task<List<TicketResponse>> GetTickets(string? searchItem = "");
    Task<List<TicketResponse>> GetTicketsByCustomerId(Guid customerId, string? searchItem = "");
}
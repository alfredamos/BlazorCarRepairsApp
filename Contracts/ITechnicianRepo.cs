using BlazorCarRepairsApp.Dto;
using BlazorCarRepairsApp.Dto.Technicians;

namespace BlazorCarRepairsApp.Contracts;

public interface ITechnicianRepo
{
    Task<ResponseMessage> CreateTech(TechCreateDto technician);
    Task<ResponseMessage> DeleteTechById(Guid id);
    Task<ResponseMessage> EditTechById(Guid id, TechEditDto technician);
    Task<TechResponse> GetTechById(Guid id);
    Task<List<TechResponse>> GetAllTechs();
    Task<TechResponse> GetTechByUserId(Guid userId);
    Task<List<TechResponse>> GetTechBySpecialty(string specialty);
}
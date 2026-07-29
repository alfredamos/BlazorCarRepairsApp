using BlazorCarRepairsApp.Dto.Technicians;
using BlazorCarRepairsApp.Models;

namespace BlazorCarRepairsApp.Mappers;

public static class TechnicianMapper
{
    public static Technician TechCreateDtoToTechnician(TechCreateDto dto)
    {
        return new Technician
        {
            Specialty = dto.Specialty,
            UserId = dto.UserId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }
    
    public static Technician TechEditDtoToTechnician(Technician tech, TechEditDto dto, DateTime createdAt)
    {
        tech.Id = dto.Id;
        tech.Specialty = dto.Specialty;
        tech.CreatedAt = createdAt;
        tech.UpdatedAt = DateTime.UtcNow;
        
        return tech;
    }

    public static TechResponse TechnicianToTechResponse(Technician tech)
    {
        return new TechResponse
        {
            Id = tech.Id,
            Name = tech.User?.Name,
            Email = tech.User?.Email,
            Phone = tech.User?.PhoneNumber,
            Gender = tech.User?.Gender,
            Birthdate = tech.User?.Birthdate,
            Image = tech.User?.ImagePath,
            Specialty = tech.Specialty,
            UserId = tech.UserId,
            CreatedAt = tech.CreatedAt,
            UpdatedAt = tech.UpdatedAt
                
        };
    }
}


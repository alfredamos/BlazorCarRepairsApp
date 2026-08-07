using System.Net;
using BlazorCarRepairsApp.Contracts;
using BlazorCarRepairsApp.Data;
using BlazorCarRepairsApp.Dto;
using BlazorCarRepairsApp.Dto.Technicians;
using BlazorCarRepairsApp.Exceptions;
using BlazorCarRepairsApp.Mappers;
using BlazorCarRepairsApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorCarRepairsApp.Repositories;

public class TechnicianRepo(ApplicationDbContext context) : ITechnicianRepo
{
    public async Task<ResponseMessage> CreateTech(TechCreateDto techDto)
    {
        //----> Map tech-create-dto to technician.
        var tech = TechnicianMapper.TechCreateDtoToTechnician(techDto);
        
        //----> Insert the technician into db.
        await context.Technicians.AddAsync(tech);
        await context.SaveChangesAsync();
        
        //----> Send back response.
        return new ResponseMessage
        {
            Message = "Technician created successfully!",
            Status = "Success",
            StatusCode = HttpStatusCode.Created,
        };
    }

    public async Task<ResponseMessage> DeleteTechById(Guid id)
    {
        //----> Check for existence of technician.
        var tech = await GetOneTechnician(id);
        
        //----> Delete the technician with the giving id.
        context.Technicians.Remove(tech);
        await context.SaveChangesAsync();
        
        //----> Send back response.
        return new ResponseMessage
        {
            Message = "Technician deleted successfully!",
            Status = "Success",
            StatusCode = HttpStatusCode.OK,
        };
    }

    public async Task<ResponseMessage> EditTechById(Guid id, TechEditDto techDto)
    {
        //----> Check for invalid id.
        if (!techDto.Id.Equals(id))
        {
            throw new CustomException("Invalid technician id!", HttpStatusCode.BadRequest); 
        }
        
        //----> Check for existence of technician.
        var tech = await GetOneTechnician(id);
        
        //----> Map technician edit dto to technician.
        tech = TechnicianMapper.TechEditDtoToTechnician(tech, techDto, tech.UpdatedAt);
        
        //----> Update the technician detail in db.
        context.Technicians.Update(tech);
        await context.SaveChangesAsync();
        
        //----> Send back response.
        return new ResponseMessage
        {
            Message = "Technician edited successfully!",
            Status = "Success",
            StatusCode = HttpStatusCode.OK,
        };
    }

    public async Task<TechResponse> GetTechById(Guid id)
    {
        //----> Fetch the technician with giving id.
        var tech = await GetOneTechnician(id);
        
        //----> Send back response.
        return TechnicianMapper.TechnicianToTechResponse(tech);
    }

    public async Task<List<TechResponse>> GetAllTechs(string? searchItem = "")
    {
        var query = context.Technicians.Include(cst => cst.User).AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchItem))
        {
            var search = searchItem.Trim().ToLower();
        
            query = query.Where(cst => 
                (cst.Specialty.ToLower().Contains(search)) ||
                (cst.User != null && (
                    (!string.IsNullOrEmpty(cst.User.Name) && cst.User.Name.ToLower().Contains(search)) ||
                    (!string.IsNullOrEmpty(cst.User.Email) && cst.User.Email.ToLower().Contains(search)) ||
                    (!string.IsNullOrEmpty(cst.User.PhoneNumber) && cst.User.PhoneNumber.Contains(search)) ||
                    (!string.IsNullOrEmpty(cst.User.Gender) && cst.User.Gender.ToLower().Contains(search))
                ))
            );
        }

        var technicians = await query.ToListAsync();

        return [.. technicians.Select(TechnicianMapper.TechnicianToTechResponse)];
    }

    public async Task<TechResponse> GetTechByUserId(Guid userId)
    {
        //----> Fetch the tech from db.
        var tech = await context.Technicians.Include(tk => tk.User).AsNoTracking().FirstOrDefaultAsync(tk => tk.UserId.Equals(userId));

        //----> Check for null tech and send back response.
        return tech is not null ? TechnicianMapper.TechnicianToTechResponse(tech) : throw new CustomException("Technician not found", HttpStatusCode.NotFound);
    }

    public async Task<List<TechResponse>> GetTechBySpecialty(string specialty, string? searchItem = "")
    {
        //----> Fetch all technicians by specialty.
        var techs = (await GetAllTechs(searchItem)).Where(tk => tk.Specialty.Contains(specialty, StringComparison.CurrentCultureIgnoreCase)).ToList();
        
        //----> Send back response.
        return techs;
    }

    private async Task<Technician> GetOneTechnician(Guid id)
    {
        //----> Fetch the tech from db.
        var tech = await context.Technicians.Include(tk => tk.User).FirstOrDefaultAsync(tk => tk.Id.Equals(id));
        
        //----> Check for null tech.
        return tech ?? throw new CustomException("Technician not found", HttpStatusCode.NotFound);
    }
}
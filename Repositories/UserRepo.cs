using System.Net;
using BlazorCarRepairsApp.Contracts.Users;
using BlazorCarRepairsApp.Data;
using BlazorCarRepairsApp.Dto;
using BlazorCarRepairsApp.Exceptions;
using BlazorCarRepairsApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlazorCarRepairsApp.Repositories;

public class UserRepo(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : IUserRepo
{
    public async Task<UserDto> DeleteUserById(Guid id)
    {
        //----> Check for existence of user.
        var user = await GetOneUser(id);
        
        //----> Delete user with the given id.
        context.Users.Remove(user);
        await context.SaveChangesAsync();
        
        //----> Send back result.
        return await ToUserDto(user);
    }

    public async Task<List<UserDto>> GetAllUsers()
    {
        //----> Fetch all users.
        var users = await context.Users.ToListAsync();
        
        //----> Send back response.
        return [.. users.Select((user) => ToUserDto(user).Result)];
    }

    public async Task<UserDto> GetUserById(Guid id)
    {
        //----> Fetch the user with the giving id.
        var user = await GetOneUser(id);
        
        //----> Send back response.
        return await ToUserDto(user);
    }

    private async Task<ApplicationUser> GetOneUser(Guid id)
    {
        //----> Fetch the user with the giving id.
        var user = await context.Users.FirstOrDefaultAsync(us => us.Id.Equals(id));
        
        return user ??  throw new CustomException("User not found", HttpStatusCode.NotFound);
    }

    private async Task<UserDto> ToUserDto(ApplicationUser user)
    {
        //----> Get the user-role.
        var role = (await userManager.GetRolesAsync(user))[0];
        
        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Phone = user.PhoneNumber,
            Image = user.ImagePath,
            Gender = user.Gender,
            Role = role
        };
    }
}
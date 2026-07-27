using System.Net;
using BlazorCarRepairsApp.Contracts.Users;
using BlazorCarRepairsApp.Data;
using BlazorCarRepairsApp.Dto;
using BlazorCarRepairsApp.Exceptions;
using BlazorCarRepairsApp.Mappers;
using BlazorCarRepairsApp.Models;
using BlazorCarRepairsApp.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlazorCarRepairsApp.Repositories;

public class UserRepo(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager) : IUserRepo
{
    public async Task<UserDto> ChangeUserRole(string email)
    {
        //----> Get user from db.
        var userToChange = await GetUserByEmail(email);

        //----> Get user roles.
        var roles = await userManager.GetRolesAsync(userToChange);
        var role = roles.FirstOrDefault() ?? Roles.User;

        //----> Change user role.
        var targetRole = role == Roles.User ? Roles.Admin : Roles.User;

        //----> Check for existence of target role.
        if (!await roleManager.RoleExistsAsync(targetRole))
            throw new CustomException("Role does not exist", HttpStatusCode.NotFound);

        //----> Remove old user role.
        var isRoleRemoved = await userManager.RemoveFromRoleAsync(userToChange, role);
        if (!isRoleRemoved.Succeeded)
            throw new CustomException("Failed to remove user role", HttpStatusCode.InternalServerError);

        //----> Update user role.
        var isUpdated = await userManager.AddToRoleAsync(userToChange, targetRole);
        return isUpdated.Succeeded
            ? UserMapper.MapUserToUserDto(userToChange, Roles.User)
            : throw new CustomException("Failed to change user role", HttpStatusCode.InternalServerError);
    }

    

    public async Task<UserDto> DeleteUserById(Guid id)
    {
        //----> Check for existence of user.
        var user = await GetOneUser(id);
     
        //----> Get the user role.
        var role = await userManager.GetRolesAsync(user);
     
        //----> Delete the user.
        var isDeleted = await userManager.DeleteAsync(user);
        return isDeleted.Succeeded
            ? UserMapper.MapUserToUserDto(user, role[0])
            : throw new CustomException("User not deleted", HttpStatusCode.InternalServerError);
    }

    public async Task<List<UserDto>> GetAllUsers()
    {
        //----> Fetch all users from the database.
        var users = await userManager.Users.ToListAsync();
        
        //----> Map users to usersDto.
        return await MapUsersToUserDtos(users);
    }

    public async Task<UserDto> GetUserById(Guid id)
    {
        //----> Check for existence of user.
        var user = await GetOneUser(id);
        
        //----> Check for null user.
        if (user == null)
        {
            throw new CustomException("User not found", HttpStatusCode.NotFound);
        }
     
        //----> Get the user role.
        var roles = await userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? Roles.User;
     
        //----> Send back response.
        return UserMapper.MapUserToUserDto(user, role);

    }
    
    private async Task<ApplicationUser> GetUserByEmail(string email)
    {
        //----> Fetch the user by email
        var user = await userManager.FindByEmailAsync(email);

        //----> Return the user
        return user ?? throw new CustomException("User not found",  HttpStatusCode.NotFound);
    }

    private async Task<ApplicationUser> GetOneUser(Guid id)
    {
        //----> Fetch the user with the given id.
        var user = await userManager.FindByIdAsync(id.ToString());
     
        //----> Return the user.
        return user ?? throw new CustomException("User not found", HttpStatusCode.NotFound);
    }

    private async Task<List<UserDto>> MapUsersToUserDtos(List<ApplicationUser> users)
    {
        var allUsers = new List<UserDto>();

        // Process each user sequentially/asynchronously to avoid thread deadlocks.
        foreach (var user in users)
        {
            var roles = await userManager.GetRolesAsync(user);
            var primaryRole = roles.FirstOrDefault() ?? Roles.User;
        
            // Maps the system user and their role string to the DTO
            var userDto = UserMapper.MapUserToUserDto(user, primaryRole);
            allUsers.Add(userDto);
        }

        // Send back response.
        return allUsers;
    }
}
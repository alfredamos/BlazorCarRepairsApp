using BlazorCarRepairsApp.Dto;
using BlazorCarRepairsApp.Models;

namespace BlazorCarRepairsApp.Mappers;

public static class UserMapper
{
    public static UserDto MapUserToUserDto(ApplicationUser user, string role)
    {
        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Image = user.ImagePath,
            Gender = user.Gender,
            Phone = user.PhoneNumber,
            Birthdate = user.Birthdate,
            Role = role,
        };
    }
    
}
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
            Birthdate = user.Birthdate,
            Name = user.Name,
            Email = user.Email,
            Phone = user.PhoneNumber,
            Image = user.ImagePath,
            Gender = user.Gender,
            Role = role,
            UserType = user.UserType
        };
    }
}
using BlazorCarRepairsApp.Dto;

namespace BlazorCarRepairsApp.Contracts;

public interface IUserRepo
{   
    Task<UserDto> ChangeUserRole(string email);
    Task<UserDto> DeleteUserById(Guid id);
    Task<List<UserDto>> GetAllUsers(string? searchItem = "");
    Task<UserDto> GetUserById(Guid id);
}
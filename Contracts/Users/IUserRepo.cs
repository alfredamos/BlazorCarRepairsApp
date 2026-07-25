using BlazorCarRepairsApp.Dto;

namespace BlazorCarRepairsApp.Contracts.Users;

public interface IUserRepo
{   
    Task<UserDto> DeleteUserById(Guid id);
    Task<List<UserDto>> GetAllUsers();
    Task<UserDto> GetUserById(Guid id);
}
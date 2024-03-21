using GRPCClient.Models;

namespace GRPCClient.Repos.UserRepo;

public interface IUserService
{
    Task<int> CreateUserAsync(CreateUserDto createUserRequest);
    Task<UserListResponse> GetUsersListAsync(int pageSize = 10, int pageNumber = 1);
    Task<UserDto> GetUserAsync(int id);
    Task<DeleteUserResponse> DeleteUserAsync(int id);
}

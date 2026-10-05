using Auth.Dtos;
using Auth.Models;

namespace Auth.Services;

public interface IUserService
{
    public Task<PagedResult<UserDto>> SearchUsersAsync(SearchUsersDto dto,  CancellationToken cancellationToken = default);
    
    public Task<User?> GetUserAsync(string username, CancellationToken cancellationToken = default);
    
    public Task<User?> UpdateAsync(string username, UpdateUserDto dto, CancellationToken cancellationToken = default);
    
    public Task<bool> DeleteAsync(string username, CancellationToken cancellationToken = default);
}
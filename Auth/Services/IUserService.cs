using Auth.Dtos;
using Auth.Models;

namespace Auth.Services;

public interface IUserService
{
    public Task<User?> GetUserAsync(string username);
    
    public Task<User?> UpdateAsync(string username, UpdateUserDto dto);
    
    public Task<bool> DeleteAsync(string username);
}
using Auth.Dtos;
using Auth.Models;

namespace Auth.Services;

public interface IAuthService
{
    public Task<User?> SignupAsync(string username, string password);
    
    public Task<User?> LoginAsync(string username, string password);
}
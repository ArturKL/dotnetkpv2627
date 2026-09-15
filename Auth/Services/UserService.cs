using Auth.Dtos;
using Auth.Models;

namespace Auth.Services;

public class UserService(Database.Database db) : IUserService
{
    public Task<User?> GetUserAsync(string username)
    {
        return Task.FromResult(db.Users.FirstOrDefault(u => u.Username == username));
    }

    public Task<User?> UpdateAsync(string username, UpdateUserDto dto)
    {
        var user = db.Users.FirstOrDefault(u => u.Username == username);
        if (user == null)
        {
            return Task.FromResult<User?>(null);
        }

        user.Description = dto.Description;
        return Task.FromResult<User?>(user);
    }

    public Task<bool> DeleteAsync(string username)
    {
        var user = db.Users.FirstOrDefault(u => u.Username == username);
        if (user == null)
        {
            return Task.FromResult<bool>(false);
        }
        var result = db.Users.Remove(user);
        return Task.FromResult(result);
    }
}
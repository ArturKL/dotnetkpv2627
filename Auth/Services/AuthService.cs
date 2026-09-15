using Auth.Dtos;
using Auth.Models;

namespace Auth.Services;

public class AuthService(Database.Database db, IPasswordHasher hasher) : IAuthService
{
    public Task<User?> SignupAsync(string username, string password)
    {
        if (db.Users.Exists(u => u.Username == username))
        {
            return Task.FromResult<User?>(null);
        }

        var user = new User { Username = username, PasswordHash = hasher.Hash(password) };
        db.Users.Add(user);

        return Task.FromResult<User?>(user);
    }

    public Task<User?> LoginAsync(string username, string password)
    {
        var user = db.Users.FirstOrDefault(u => u.Username == username);
        if (user == null)
        {
            return Task.FromResult<User?>(null);
        }

        return Task.FromResult(!hasher.Verify(password, user.PasswordHash) ? null : user);
    }
}
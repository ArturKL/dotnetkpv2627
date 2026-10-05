using Auth.Database;
using Auth.Dtos;
using Auth.Models;
using Microsoft.EntityFrameworkCore;

namespace Auth.Services;

public class AuthService(AppDbContext db, IPasswordHasher hasher) : IAuthService
{
    public async Task<User?> SignupAsync(string username, string password, CancellationToken cancellationToken)
    {
        var existingUser = await db.Users.FirstOrDefaultAsync(u => u.Username == username, cancellationToken);
        if (existingUser != null)
        {
            return null;
        }

        var user = new User { Username = username, PasswordHash = hasher.Hash(password) };
        db.Users.Add(user);
        await db.SaveChangesAsync(cancellationToken);
        return user;
    }

    public async Task<User?> LoginAsync(string username, string password, CancellationToken cancellationToken)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.Username == username, cancellationToken);
        if (user == null)
        {
            return null;
        }

        return !hasher.Verify(password, user.PasswordHash) ? null : user;
    }
}
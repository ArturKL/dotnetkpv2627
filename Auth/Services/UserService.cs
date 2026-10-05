using Auth.Database;
using Auth.Dtos;
using Auth.Models;
using Microsoft.EntityFrameworkCore;

namespace Auth.Services;

public class UserService(AppDbContext db) : IUserService
{
    public async Task<User?> GetUserAsync(string username, CancellationToken cancellationToken)
    {
        return await db.Users.FirstOrDefaultAsync(u => u.Username == username,  cancellationToken);
    }

    public async Task<User?> UpdateAsync(string username, UpdateUserDto dto, CancellationToken cancellationToken)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.Username == username, cancellationToken);
        if (user == null)
        {
            return null;
        }

        user.Description = dto.Description;
        await db.SaveChangesAsync(cancellationToken);
        return user;
    }

    public async Task<bool> DeleteAsync(string username, CancellationToken cancellationToken)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.Username == username, cancellationToken);
        if (user == null)
        {
            return false;
        }
        var result = db.Users.Remove(user);
        await db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
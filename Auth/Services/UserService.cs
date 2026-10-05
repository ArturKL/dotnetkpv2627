using Auth.Database;
using Auth.Dtos;
using Auth.Models;
using Microsoft.EntityFrameworkCore;

namespace Auth.Services;

public class UserService(AppDbContext db) : IUserService
{
    public async Task<PagedResult<UserDto>> SearchUsersAsync(SearchUsersDto dto,
        CancellationToken cancellationToken = default)
    {
        var query = db.Users.AsQueryable();

        if (dto.CreatedAtRange.From.HasValue)
        {
            query = query.Where(u =>
                u.CreatedAt >= dto.CreatedAtRange.From.Value);
        }

        if (dto.CreatedAtRange.To.HasValue)
        {
            query = query.Where(u =>
                u.CreatedAt <= dto.CreatedAtRange.To.Value);
        }

        if (dto.UpdatedAtRange.From.HasValue)
        {
            query = query.Where(u =>
                u.UpdatedAt >= dto.UpdatedAtRange.From.Value);
        }

        if (dto.UpdatedAtRange.To.HasValue)
        {
            query = query.Where(u =>
                u.UpdatedAt <= dto.UpdatedAtRange.To.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var users = await query
            .OrderBy(u => u.Id)
            .Skip((dto.PageNumber - 1) * dto.PageSize)
            .Take(dto.PageSize)
            .Select(u => new UserDto(u.Username, u.Description))
            .ToListAsync(cancellationToken);

        return new PagedResult<UserDto>
        {
            TotalCount = totalCount,
            Items = users,
            PageSize = dto.PageSize,
            Page = dto.PageNumber
        };
    }

    public async Task<User?> GetUserAsync(string username, CancellationToken cancellationToken)
    {
        return await db.Users.FirstOrDefaultAsync(u => u.Username == username, cancellationToken);
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
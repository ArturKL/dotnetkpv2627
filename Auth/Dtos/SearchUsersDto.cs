using System.Linq.Expressions;

namespace Auth.Dtos;

public sealed class Range<T> where T : struct, IComparable<T>
{
    public T? From { get; set; }
    public T? To { get; set; }

    public bool IsInRange(T value) =>
        (!From.HasValue || value.CompareTo(From.Value) >= 0) &&
        (!To.HasValue || value.CompareTo(To.Value) <= 0);
}

public sealed class PagedResult<T>
{
    public long TotalCount { get; set; }
    public IReadOnlyList<T> Items { get; set; } = [];
    public int Page { get; init; }
    public int PageSize { get; init; }

    public int TotalPages =>
        (int)Math.Ceiling((double)TotalCount / PageSize);
}

public class SearchUsersDto
{
    public Range<DateTime> CreatedAtRange { get; set; }
    public Range<DateTime> UpdatedAtRange { get; set; }

    public int PageSize { get; set; } = 20;
    public int PageNumber { get; set; } = 1;
}
namespace TaskManager.Domain.DTOs.Common;

public record PagedResponse<T>(
    int Page,
    int PageSize,
    int TotalItems,
    IEnumerable<T> Items
);
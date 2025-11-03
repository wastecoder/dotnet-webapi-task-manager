using TaskManager.Domain.Enums;

namespace TaskManager.Domain.DTOs.Task;

public record UpdateTaskDto(
    int Id,
    string Title,
    string Description,
    DateTime DueDate,
    ETaskStatus Status
);
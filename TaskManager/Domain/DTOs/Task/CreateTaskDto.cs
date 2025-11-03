using TaskManager.Domain.Enums;

namespace TaskManager.Domain.DTOs.Task;

public record CreateTaskDto(
    string Title,
    string Description,
    DateTime DueDate,
    ETaskStatus Status
);
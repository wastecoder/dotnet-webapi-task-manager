using TaskManager.Domain.Enums;

namespace TaskManager.Domain.DTOs.Task;

public record UpdateTaskDto(
    string Title,
    string Description,
    DateTime DueDate,
    ETaskStatus Status
);
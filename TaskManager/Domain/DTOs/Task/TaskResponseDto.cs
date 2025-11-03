using TaskManager.Domain.Enums;

namespace TaskManager.Domain.DTOs.Task;

public record TaskResponseDto(
    int Id,
    string Title,
    string Description,
    DateTime DueDate,
    ETaskStatus Status
);
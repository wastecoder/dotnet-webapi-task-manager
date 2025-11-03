using TaskManager.Domain.Entities;
using TaskManager.Domain.DTOs.Task;

namespace TaskManager.Mappers;

public static class TaskMapper
{
    // TaskItem -> TaskResponseDto
    public static TaskResponseDto ToResponseDto(this TaskItem task) =>
        new(task.Id, task.Title, task.Description, task.DueDate, task.Status);

    // CreateTaskDto -> TaskItem
    public static TaskItem ToEntity(this CreateTaskDto dto) =>
        new()
        {
            Title = dto.Title,
            Description = dto.Description,
            DueDate = dto.DueDate,
            Status = dto.Status
        };

    // UpdateTaskDto -> TaskItem (para atualizar)
    public static void UpdateEntity(this TaskItem task, UpdateTaskDto dto)
    {
        task.Title = dto.Title;
        task.Description = dto.Description;
        task.DueDate = dto.DueDate;
        task.Status = dto.Status;
    }
}
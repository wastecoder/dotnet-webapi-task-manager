using TaskManager.Domain.DTOs.Common;
using TaskManager.Domain.DTOs.Task;
using TaskManager.Domain.Enums;

namespace TaskManager.Domain.Interfaces;

public interface ITaskService
{
    Task<TaskResponseDto> Create(CreateTaskDto dto);
    Task<PagedResponse<TaskResponseDto>> GetAll(string? title, ETaskStatus? status, DateTime? dueDate, int page, int pageSize);
    Task<TaskResponseDto?> GetById(int id);
    Task<TaskResponseDto?> Update(int id, UpdateTaskDto dto);
    Task<bool> Delete(int id);
}
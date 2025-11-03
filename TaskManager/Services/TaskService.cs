using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.DTOs.Common;
using TaskManager.Domain.DTOs.Task;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Interfaces;
using TaskManager.Infrastructure.Database;
using TaskManager.Mappers;

namespace TaskManager.Services;

public class TaskService(TaskDbContext _context) : ITaskService
{
    public async Task<TaskResponseDto> Create(CreateTaskDto newTaskRequest)
    {
        var task = newTaskRequest.ToEntity();
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        return task.ToResponseDto();
    }

    public async Task<PagedResponse<TaskResponseDto>> GetAll(
            string? title,
            ETaskStatus? status,
            DateTime? dueDate,
            int page,
            int pageSize)
    {
        var query = _context.Tasks.AsQueryable();

        if (!string.IsNullOrWhiteSpace(title))
            query = query.Where(t => t.Title.Contains(title));

        if (status.HasValue)
            query = query.Where(t => t.Status == status.Value);

        if (dueDate.HasValue)
        {
            var startOfDayUtc = DateTime.SpecifyKind(dueDate.Value.Date, DateTimeKind.Utc);
            var endOfDayUtc = startOfDayUtc.AddDays(1);
            query = query.Where(t => t.DueDate >= startOfDayUtc && t.DueDate < endOfDayUtc);
        }

        var totalItems = await query.CountAsync();
        var tasks = await query
            .OrderBy(t => t.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var response = tasks.Select(t => t.ToResponseDto());

        return new PagedResponse<TaskResponseDto>(
            Page: page,
            PageSize: pageSize,
            TotalItems: totalItems,
            Items: response
        );
    }

    public async Task<TaskResponseDto?> GetById(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        return task?.ToResponseDto();
    }

    public async Task<TaskResponseDto?> Update(int id, UpdateTaskDto dto)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null) return null;

        task.UpdateEntity(dto);
        await _context.SaveChangesAsync();

        return task.ToResponseDto();
    }

    public async Task<bool> Delete(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null) return false;

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
        return true;
    }
}
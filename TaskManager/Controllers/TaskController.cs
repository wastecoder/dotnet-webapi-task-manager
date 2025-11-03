using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.DTOs.Common;
using TaskManager.Domain.DTOs.Task;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Factories;
using TaskManager.Infrastructure.Database;
using TaskManager.Mappers;

namespace TaskManager.Controllers;

[ApiController]
[Route("[controller]")]
public class TaskController : ControllerBase
{
    private readonly TaskDbContext _context;
    public TaskController(TaskDbContext dbContext)
    {
        _context = dbContext;
    }

    [HttpPost]
    public IActionResult Post([FromBody] CreateTaskDto newTaskDto)
    {
        var newTask = newTaskDto.ToEntity();

        _context.Add(newTask);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetById), new { id = newTask.Id }, newTask.ToResponseDto());
    }

    [HttpGet]
    public async Task<ActionResult<PagedResponse<TaskResponseDto>>> Get(
        [FromQuery] string? title,
        [FromQuery] ETaskStatus? status,
        [FromQuery] DateTime? dueDate,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 5)
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

        return Ok(new PagedResponse<TaskResponseDto>(
            Page: page,
            PageSize: pageSize,
            TotalItems: totalItems,
            Items: response
        ));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TaskResponseDto>> GetById(int id)
    {
        var task = await _context.Tasks.FindAsync(id);

        if (task == null)
            return NotFound(ProblemFactory.NotFound(
                detail: $"Task with ID {id} not found.",
                instance: HttpContext.Request.Path
            ));

        return Ok(task.ToResponseDto());
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] UpdateTaskDto updateDto)
    {
        var existingTask = _context.Tasks.Find(id);
        if (existingTask == null)
        {
            return NotFound(ProblemFactory.NotFound(
                detail: $"Task with ID {id} not found.",
                instance: HttpContext.Request.Path
            ));
        }

        existingTask.UpdateEntity(updateDto);
        _context.SaveChanges();

        return Ok(existingTask.ToResponseDto());
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var existingTask = _context.Tasks.Find(id);
        if (existingTask == null)
        {
            return NotFound(ProblemFactory.NotFound(
                detail: $"Task with ID {id} not found.",
                instance: HttpContext.Request.Path
            ));
        }

        _context.Tasks.Remove(existingTask);
        _context.SaveChanges();

        return NoContent();
    }
}
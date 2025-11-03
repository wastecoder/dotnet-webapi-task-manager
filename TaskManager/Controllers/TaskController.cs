using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.DTOs.Task;
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

        return CreatedAtAction(nameof(Post), new { id = newTask.Id }, newTask.ToResponseDto());
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskResponseDto>>> GetAll(
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 5)
    {
        if (page <= 0) page = 1;
        if (pageSize <= 0) pageSize = 5;

        var tasks = await _context.Tasks
            .OrderBy(t => t.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var response = tasks.Select(t => t.ToResponseDto());
        return Ok(response);
    }
}
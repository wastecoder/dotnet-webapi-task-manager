using Microsoft.AspNetCore.Mvc;
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
}
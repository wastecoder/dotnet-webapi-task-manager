using Microsoft.AspNetCore.Mvc;
using TaskManager.Domain.DTOs.Task;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Interfaces;
using TaskManager.Domain.Factories;
using TaskManager.Domain.DTOs.Common;

namespace TaskManager.Controllers;

[ApiController]
[Route("tasks")]
public class TaskController(ITaskService _taskService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateTaskDto newTaskDto)
    {
        var newTask = await _taskService.Create(newTaskDto);
        return CreatedAtAction(nameof(GetById), new { id = newTask.Id }, newTask);
    }

    [HttpGet]
    public async Task<ActionResult<PagedResponse<TaskResponseDto>>> Get(
        [FromQuery] string? title,
        [FromQuery] ETaskStatus? status,
        [FromQuery] DateTime? dueDate,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 5)
    {
        var response = await _taskService.GetAll(title, status, dueDate, page, pageSize);
        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var task = await _taskService.GetById(id);
        if (task == null)
            return NotFound(ProblemFactory.NotFound(
                detail: $"Task with ID {id} not found.",
                instance: HttpContext.Request.Path
            ));

        return Ok(task);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskDto dto)
    {
        var updated = await _taskService.Update(id, dto);
        if (updated == null)
            return NotFound(ProblemFactory.NotFound(
                detail: $"Task with ID {id} not found.",
                instance: HttpContext.Request.Path
            ));

        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _taskService.Delete(id);
        if (!deleted)
            return NotFound(ProblemFactory.NotFound(
                detail: $"Task with ID {id} not found.",
                instance: HttpContext.Request.Path
            ));

        return NoContent();
    }
}
using EmployeeTaskApi.DTOs;
using EmployeeTaskApi.Models;
using EmployeeTaskApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeTaskApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly ITaskService _service;
    public TasksController(ITaskService service) => _service = service;

    /// <summary>List tasks with search, filters (status, priority, employee) and pagination.</summary>
    [HttpGet]
    public async Task<ActionResult<PagedResult<TaskDto>>> GetAll(
        [FromQuery] string? search, [FromQuery] TaskItemStatus? status, [FromQuery] TaskPriority? priority,
        [FromQuery] int? employeeId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, 100);
        return Ok(await _service.GetAllAsync(search, status, priority, employeeId, page, pageSize));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TaskDto>> GetById(int id)
    {
        var t = await _service.GetByIdAsync(id);
        return t is null ? NotFound() : Ok(t);
    }

    [HttpPost, Authorize(Roles = "Admin")]
    public async Task<ActionResult<TaskDto>> Create(TaskCreateDto dto)
    {
        var (result, error) = await _service.CreateAsync(dto);
        if (error is not null) return BadRequest(new { message = error });
        return CreatedAtAction(nameof(GetById), new { id = result!.Id }, result);
    }

    [HttpPut("{id:int}"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, TaskUpdateDto dto)
    {
        var (found, error) = await _service.UpdateAsync(id, dto);
        if (!found) return NotFound();
        if (error is not null) return BadRequest(new { message = error });
        return NoContent();
    }

    /// <summary>Any logged-in user can update a task's status.</summary>
    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(int id, TaskStatusUpdateDto dto) =>
        await _service.UpdateStatusAsync(id, dto.Status) ? NoContent() : NotFound();

    [HttpDelete("{id:int}"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id) =>
        await _service.DeleteAsync(id) ? NoContent() : NotFound();
}

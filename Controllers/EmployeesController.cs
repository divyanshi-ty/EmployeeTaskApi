using EmployeeTaskApi.DTOs;
using EmployeeTaskApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeTaskApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _service;
    public EmployeesController(IEmployeeService service) => _service = service;

    /// <summary>List employees with search, department filter and pagination.</summary>
    [HttpGet]
    public async Task<ActionResult<PagedResult<EmployeeDto>>> GetAll(
        [FromQuery] string? search, [FromQuery] string? department,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, 100);
        return Ok(await _service.GetAllAsync(search, department, page, pageSize));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<EmployeeDto>> GetById(int id)
    {
        var e = await _service.GetByIdAsync(id);
        return e is null ? NotFound() : Ok(e);
    }

    [HttpPost, Authorize(Roles = "Admin")]
    public async Task<ActionResult<EmployeeDto>> Create(EmployeeCreateDto dto)
    {
        var (result, error) = await _service.CreateAsync(dto);
        if (error is not null) return Conflict(new { message = error });
        return CreatedAtAction(nameof(GetById), new { id = result!.Id }, result);
    }

    [HttpPut("{id:int}"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, EmployeeUpdateDto dto)
    {
        var (found, error) = await _service.UpdateAsync(id, dto);
        if (!found) return NotFound();
        if (error is not null) return Conflict(new { message = error });
        return NoContent();
    }

    [HttpDelete("{id:int}"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id) =>
        await _service.DeleteAsync(id) ? NoContent() : NotFound();
}

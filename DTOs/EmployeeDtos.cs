using System.ComponentModel.DataAnnotations;

namespace EmployeeTaskApi.DTOs;

public class EmployeeCreateDto
{
    [Required, MaxLength(100)] public string FullName { get; set; } = string.Empty;
    [Required, EmailAddress, MaxLength(150)] public string Email { get; set; } = string.Empty;
    [Required, MaxLength(100)] public string Department { get; set; } = string.Empty;
    [Required, MaxLength(100)] public string Designation { get; set; } = string.Empty;
}

public class EmployeeUpdateDto : EmployeeCreateDto { }

public record EmployeeDto(int Id, string FullName, string Email, string Department, string Designation, DateTime CreatedAt);

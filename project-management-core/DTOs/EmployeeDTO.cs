namespace ProjectManagement.Core.DTOs;

public class EmployeeDTO
{
    public Id Id { get; set; }
    public string Name { get; set; }
    public string PersonalId { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public double Salary { get; set; }

    public string JobTitle { get; set; }
    public Guid? SupervisorId { get; set; }

    public static implicit operator EmployeeDTO(Employee other) =>
        new()
        {
            Id = other.Id,
            Name = other.Name,
            PersonalId = other.PersonalId,
            Email = other.Email,
            Phone = other.Phone,
            Salary = other.Salary,
            JobTitle = other.JobTitle.Name,
            SupervisorId = other.Supervisor?.Id.Value
        };
}
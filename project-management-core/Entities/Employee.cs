namespace ProjectManagement.Core.Entities;

public class Employee : IEntity
{
    public Id Id { get; set; }
    public string Name { get; set; }
    public string PersonalId { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public double Salary { get; set; }

    public virtual JobTitle JobTitle { get; set; }
    public virtual Employee? Supervisor { get; set; }
    public virtual ICollection<Employee> Supervisee { get; set; }
    public virtual ICollection<Project> Projects { get; set; }
}
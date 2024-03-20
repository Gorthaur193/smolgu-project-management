namespace ProjectManagement.Core.Entities;

public class Project : IEntity
{
    public Id Id { get; set; }
    public string Name { get; set; }
    public DateTime Deadline { get; set; }
    public double Budget { get; set; }

    public virtual ICollection<Employee> Employees { get; set; }
    public virtual ICollection<Device> Devices { get; set; }
}
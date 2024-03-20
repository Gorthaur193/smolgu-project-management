namespace ProjectManagement.Core.Entities;

public class Device : IEntity
{
    public Id Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Productivity { get; set; }
    public double Capability { get; set; }

    public virtual ICollection<Project> Projects { get; set; }
}
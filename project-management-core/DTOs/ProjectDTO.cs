namespace ProjectManagement.Core.DTOs;

public class ProjectDTO 
{
    public Id Id { get; set; }
    public string Name { get; set; }
    public DateTime Deadline { get; set; }
    public double Budget { get; set; }

    public static implicit operator ProjectDTO(Project other) =>
        new()
        {
            Budget = other.Budget,
            Name = other.Name,
            Deadline = other.Deadline,
            Id = other.Id
        };
}
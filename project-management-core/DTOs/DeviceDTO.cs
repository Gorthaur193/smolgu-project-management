namespace ProjectManagement.Core.DTOs;

public class DeviceDTO
{
    public Id Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Productivity { get; set; }
    public double Capability { get; set; }

    public static implicit operator DeviceDTO(Device other) =>
        new()
        {
            Id = other.Id,
            Name = other.Name,
            Description = other.Description,
            Productivity = other.Productivity,
            Capability = other.Capability
        };
}
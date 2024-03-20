namespace ProjectManagement.Core.Entities;

public class JobTitle : IEntity
{
    public Id Id { get; set; }
    public string Name { get; set; }
}
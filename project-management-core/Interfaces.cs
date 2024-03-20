namespace ProjectManagement.Core;

public record Id(Guid Value);
public interface IEntity
{
    Id Id { get; }
}
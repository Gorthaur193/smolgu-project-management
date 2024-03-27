namespace ProjectManagement.Core.Exceptions;

public class SimilarJobTitleException(string newName) : Exception
{
    public override string Message => $"There is already job title with {newName}";
}
namespace ProjectManagement.Core.Exceptions;

public class ProjectNotFoundException(Guid wrongId) : Exception($"Project {wrongId} is not found.") {}

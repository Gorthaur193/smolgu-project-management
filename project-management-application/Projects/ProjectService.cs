namespace ProjectManagement.Application.Projects;

public class ProjectService(IRepository<Project> repository) : IServise
{
    private IRepository<Project> Repository { get; init; } = repository;

    public async Task<IEnumerable<ProjectDTO>> GetProjects(CancellationToken cancellationToken = default) =>
        (await Repository.Get(cancellationToken)).Cast<ProjectDTO>();

    public async Task<IEnumerable<DeviceDTO>> GetProjectsDevices(Guid projectId, CancellationToken cancellationToken = default)
    {
        var potentialProject = (await Repository.GetWithoutTracking(x => x.Id.Value == projectId, cancellationToken)).FirstOrDefault() ??
            throw new ProjectNotFoundException(projectId);
        return potentialProject.Devices.Cast<DeviceDTO>();
    }

    public async Task<IEnumerable<EmployeeDTO>> GetProjectsEmployees(Guid projectId, CancellationToken cancellationToken = default)
    {
        var potentialProject = (await Repository.GetWithoutTracking(x => x.Id.Value == projectId, cancellationToken)).FirstOrDefault() ??
            throw new ProjectNotFoundException(projectId);
        return potentialProject.Devices.Cast<EmployeeDTO>();
    }

    public async Task CreateOrUpdateProject(ProjectDTO project, CancellationToken cancellationToken = default)
    {
        Project localProj;
        if (project.Id is not null)
        {
            localProj = (await Repository.Get(x => x.Id.Value == project.Id.Value, cancellationToken)).FirstOrDefault() ??
                throw new ProjectNotFoundException(project.Id.Value);
            localProj.Name = project.Name;
            localProj.Budget = project.Budget;
            localProj.Deadline = project.Deadline;
        }
        else
            localProj = new()
            {
                Name = project.Name,
                Budget = project.Budget,
                Deadline = project.Deadline,
            };

        if (localProj.Id is null)
            await Repository.Add(localProj, cancellationToken);
        else
            await Repository.Update(localProj, cancellationToken);
    }
}
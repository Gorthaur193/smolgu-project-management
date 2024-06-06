namespace ProjectManagement.Application.Projects;

public class ProjectService(IRepository<Project> repository) : IService
{
    private IRepository<Project> Repository { get; init; } = repository;

    public async Task<IEnumerable<ProjectDTO>> GetProjectsAsync(CancellationToken cancellationToken = default) =>
        (await Repository.Get(cancellationToken)).Select(x => (ProjectDTO)x);

    public async Task<IEnumerable<DeviceDTO>> GetProjectsDevicesAsync(Guid projectId, CancellationToken cancellationToken = default)
    {
        var potentialProject = (await Repository.GetWithoutTracking(x => x.Id.Value == projectId, cancellationToken)).FirstOrDefault() ??
            throw new ProjectNotFoundException(projectId);
        return potentialProject.Devices.Cast<DeviceDTO>();
    }

    public async Task<IEnumerable<EmployeeDTO>> GetProjectsEmployeesAsync(Guid projectId, CancellationToken cancellationToken = default)
    {
        var potentialProject = (await Repository.GetWithoutTracking(x => x.Id.Value == projectId, cancellationToken)).FirstOrDefault() ??
            throw new ProjectNotFoundException(projectId);
        return potentialProject.Devices.Cast<EmployeeDTO>();
    }

    public async Task CreateOrUpdateProjectAsync(ProjectDTO project, CancellationToken cancellationToken = default)
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
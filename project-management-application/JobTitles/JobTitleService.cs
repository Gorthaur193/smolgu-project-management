namespace ProjectManagement.Application.JobTitles;

public class JobTitleService(IRepository<JobTitle> repository) : IServise
{
    private IRepository<JobTitle> Repository { get; init; } = repository;

    public Task<IEnumerable<JobTitle>> GetJobTitlesAsync(CancellationToken cancellationToken = default) =>
        Repository.Get(cancellationToken);

    public async Task AddJobTitleAsync(string newTitle, CancellationToken cancellationToken = default)
    {
        var potentialCopies = await Repository.GetWithoutTracking(x => x.Name.Equals(newTitle, StringComparison.CurrentCultureIgnoreCase), cancellationToken);
        if (potentialCopies.Any())
            throw new SimilarJobTitleException(newTitle);
        await Repository.Add(new() { Name = newTitle }, cancellationToken);
    }
}
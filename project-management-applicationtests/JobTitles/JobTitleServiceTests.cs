using ProjectManagement.Application.JobTitles;
using ProjectManagement.Core;
using ProjectManagement.Core.Entities;
using ProjectManagement.Core.Exceptions;

namespace project_management_applicationtests.JobTitles;

public class JobTitleServiceTests
{

    [TestCase("name1", "Name1")]
    [TestCase("naMe1", "Name1")]
    [TestCase("NamE1", "Name1")]
    public void AddJobTitle_WhenSameName_ShouldThrowException(string name1, string name2)
    {
        // Arrange
        var jobTitleRepo = new FakeRepository<JobTitle>();
        jobTitleRepo.AddRange([ new JobTitle() { Id = new Id(Guid.NewGuid()), Name = name1 } ]);
        var jobTitleService = new JobTitleService(jobTitleRepo);

        // Act
        AsyncTestDelegate act = async delegate { await jobTitleService.AddJobTitleAsync(name2); };

        // Assert
        Assert.ThrowsAsync<SimilarJobTitleException>(act);
    }
}

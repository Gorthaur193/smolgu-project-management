using ProjectManagement.Application.Devices;
using ProjectManagement.Application.Projects;
using ProjectManagement.Core;
using ProjectManagement.Core.DTOs;
using ProjectManagement.Core.Entities;
using ProjectManagement.Core.Exceptions;

namespace project_management_applicationtests.Projects;

public class ProjectServiceTests
{
    [Test]
    public async Task CreateOrUpdateProject_WhenProjectForUpdateDoesntExists_ThrowsException()
    {
        // Arrange
        var repo = new FakeRepository<Project>();
        var project = new Project
        {
            Id = new Id(Guid.NewGuid()),
            Name = "Test"
        };
        await repo.AddRange([project]);

        var service = new ProjectService(repo); 
        var toChange = new Project { Id = new Id(Guid.NewGuid()), Name = "NewName" };

        // Act
        AsyncTestDelegate act = async delegate { await service.CreateOrUpdateProjectAsync(toChange); };

        // Assert
        Assert.ThrowsAsync<ProjectNotFoundException>(act);
    }

    [TestCase("oldname", 10.1, "2020-1-1", "newname", 10.1, "2020-1-1")]
    [TestCase("oldname", 10.1, "2020-1-1", "oldname", 1.5, "2020-1-1")]
    [TestCase("oldname", 10.1, "2020-1-1", "oldname", 10.1, "2022-5-20")]
    [TestCase("oldname", 10.1, "2020-1-1", "newname", 1.1, "2022-5-20")]
    public async Task CreateOrUpdateProject_WhenProjectForUpdateExists_ProjectIsUpdated(
        string oldName, double oldBudget, DateTime oldDeadline,
        string newName, double newBudget, DateTime newDeadline)
    {
        // Arrange
        var repo = new FakeRepository<Project>();
        var project = new Project
        {
            Id = new Id(Guid.NewGuid()),
            Name = oldName, 
            Budget = oldBudget,
            Deadline = oldDeadline
        };
        await repo.AddRange([project]);

        var service = new ProjectService(repo);
        var toChange = new Project { Id = project.Id, Name = newName, Budget = newBudget, Deadline = newDeadline  };

        // Act
        await service.CreateOrUpdateProjectAsync(toChange);

        // Assert
        var data = await repo.Get();
        var newProj = data.First();

        Assert.That(data.Count() == 1 && 
                    newProj.Id == project.Id && 
                    newProj.Name == newName &&
                    newProj.Budget == newBudget &&
                    newProj.Deadline == newDeadline, Is.True);
    }

    [Test]
    public async Task CreateOrUpdateProject_WhenNewProject_ProjectCreated()
    {
        // Arrange
        var bogus = new Bogus.Faker<ProjectDTO>();
        bogus.CustomInstantiator(x => new ProjectDTO
        {
            Name = x.Name.FirstName(),
            Budget = x.Random.Double(1000, 1000000),
            Deadline = x.Date.Future()
        });

        var repo = new FakeRepository<Project>();
        var service = new ProjectService(repo);

        // Act
        await service.CreateOrUpdateProjectAsync(bogus.Generate());

        // Assert
        var elementCount = (await repo.Get()).Count();
        Assert.That(elementCount, Is.EqualTo(1));
    }

    [Test]
    public async Task GetProjects_WhenGet_ReturnedAll()
    {
        // Arrange
        var bogus = new Bogus.Faker<Project>();
        bogus.CustomInstantiator(x => new()
        {
            Id = new Id(Guid.NewGuid()),
            Name = x.Name.FirstName(),
            Budget = x.Random.Double(1000, 1000000),
            Deadline = x.Date.Future()
        });
        var data = Enumerable.Range(0, 20).Select(_ => bogus.Generate()).ToArray();

        var repo = new FakeRepository<Project>();
        await repo.AddRange(data);

        var service = new ProjectService(repo);

        // Act
        var serviceData = await service.GetProjectsAsync();

        // Assert

        Assert.That(serviceData.Select(x =>
                    data.FirstOrDefault(y => x.Id == y.Id) is not null)
                                       .All(x => x) 
                    && serviceData.Count() == data.Count(),
                    Is.True);

    }
}

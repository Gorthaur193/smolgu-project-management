using ProjectManagement.Application.Devices;
using ProjectManagement.Core;
using ProjectManagement.Core.DTOs;
using ProjectManagement.Core.Entities;

namespace project_management_applicationtests.Devices;

public class DeviceServiceTests
{
    [TestCase(10)]
    [TestCase(5)]
    [TestCase(1)]
    public async Task DeviceService_WhenOnlyToAdd_AllObjectsAdded(int count)
    {
        // Arrange
        var bogus = new Bogus.Faker<Device>();
        bogus.CustomInstantiator(x => new()
        {
            Name = x.Name.FirstName(),
            Capability = x.Random.Double(0, 25),
            Productivity = x.Random.Double(0.5, 1),
            Description = x.Company.CompanyName()
        });

        var repo = new FakeRepository<Device>();
        var service = new DeviceService(repo);

        var data = Enumerable.Range(0, count).Select(_ => (DeviceDTO)bogus.Generate()).ToArray();

        // Act
        await service.RegisterOrUpdateDevicesAsync(data);

        // Assert
        var updatedRepoDevices = await repo.Get();

        Assert.That(data.Select(x => 
                    updatedRepoDevices.FirstOrDefault(y => x.Name == y.Name) is not null)
                                       .All(x => x), 
                    Is.True);
    }

    [TestCase(10)]
    [TestCase(5)]
    [TestCase(1)]
    public async Task DeviceService_WhenOnlyToUpdate_AllObjectsUpdated(int count)
    {
        // Arrange
        var bogus = new Bogus.Faker<Device>();
        bogus.CustomInstantiator(x => new()
        {
            Id = new Id(Guid.NewGuid()),
            Name = x.Name.FirstName(),
            Capability = x.Random.Double(0, 25),
            Productivity = x.Random.Double(0.5, 1),
            Description = x.Company.CompanyName()
        });
        var data = Enumerable.Range(0, count).Select(_ => bogus.Generate()).ToArray();

        var repo = new FakeRepository<Device>();
        await repo.AddRange(data);
        
        var service = new DeviceService(repo);

        var dataForUpdate = data.Select(x => new DeviceDTO()
        {
            Id = x.Id,
            Name = bogus.Generate().Name,
            Capability = x.Capability,
            Productivity = x.Productivity,
            Description = bogus.Generate().Description,
        }).ToArray();
        // Act
        await service.RegisterOrUpdateDevicesAsync(dataForUpdate);

        // Assert
        var updatedRepoDevices = await repo.Get();

        Assert.That(dataForUpdate.Select(x =>
                    updatedRepoDevices.FirstOrDefault(y => x.Name == y.Name) is not null)
                                       .All(x => x),
                    Is.True);
    }

    [Test]
    public async Task DeviceService_WhenGet_AllObjectsReturned()
    {
        // Arrange
        var bogus = new Bogus.Faker<Device>();
        bogus.CustomInstantiator(x => new()
        {
            Id = new Id(Guid.NewGuid()),
            Name = x.Name.FirstName(),
            Capability = x.Random.Double(0, 25),
            Productivity = x.Random.Double(0.5, 1),
            Description = x.Company.CompanyName()
        });
        var data = Enumerable.Range(0, 20).Select(_ => bogus.Generate()).ToArray();

        var repo = new FakeRepository<Device>();
        await repo.AddRange(data);

        var service = new DeviceService(repo);

        // Act
        var serviceData = await service.GetDevicesAsync();

        // Assert

        Assert.That(serviceData.Select(x =>
                    data.FirstOrDefault(y => x.Id == y.Id) is not null)
                                       .All(x => x)
                    && serviceData.Count() == data.Count(),
                    Is.True);

    }
}
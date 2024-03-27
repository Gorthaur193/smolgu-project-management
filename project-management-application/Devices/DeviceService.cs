namespace ProjectManagement.Application.Devices;

public class DeviceService(IRepository<Device> repository) : IServise
{
    private IRepository<Device> Repository { get; init; } = repository;

    public async Task<IEnumerable<DeviceDTO>> GetDevices(CancellationToken cancellationToken = default) =>
        (await Repository.Get(cancellationToken)).Cast<DeviceDTO>();
}

namespace ProjectManagement.Application.Devices;

public class DeviceService(IRepository<Device> repository) : IServise
{
    private IRepository<Device> Repository { get; init; } = repository;

    public async Task<IEnumerable<DeviceDTO>> GetDevicesAsync(CancellationToken cancellationToken = default) =>
        (await Repository.Get(cancellationToken)).Select(x => (DeviceDTO)x);

    public async Task RegisterOrUpdateDevicesAsync(IEnumerable<DeviceDTO> devices, CancellationToken cancellationToken = default)
    {
        var newDevices = from device in devices
                         where device.Id is null
                         select new Device()
                         {
                             Capability = device.Capability,
                             Description = device.Description,
                             Name = device.Name,
                             Productivity = device.Productivity
                         };

        var repo = await Repository.Get(cancellationToken);
        var newOldDeviceCouples = from device in (from device in devices where device.Id is not null select device)
                                  join repDevice in repo on device.Id equals repDevice.Id
                                  select (device, repDevice);

        foreach (var (updatedDevice, oldDevice) in newOldDeviceCouples)
        {
            oldDevice.Name = updatedDevice.Name;
            oldDevice.Capability = updatedDevice.Capability;
            oldDevice.Description = updatedDevice.Description;
            oldDevice.Productivity = updatedDevice.Productivity;
        }

        // todo: validation object
        await Repository.AddRange(newDevices, cancellationToken);
        await Repository.UpdateRange(repo, cancellationToken);
    }        
}
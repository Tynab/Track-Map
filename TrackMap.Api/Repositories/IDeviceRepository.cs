using TrackMap.Api.Entities;

namespace TrackMap.Api.Repositories;

public interface IDeviceRepository
{
    public ValueTask<IEnumerable<Device>> GetAll();

    public ValueTask<Device?> Get(Guid id);

    public ValueTask<Device?> Create(Device request);

    public ValueTask<Device?> Update(Device request);
}

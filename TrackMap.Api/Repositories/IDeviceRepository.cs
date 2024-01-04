using TrackMap.Api.Entities;
using TrackMap.Common.Dtos.Device;
using TrackMap.Common.SeedWork;

namespace TrackMap.Api.Repositories;

public interface IDeviceRepository
{
    public ValueTask<IEnumerable<Device>> GetAll();

    public ValueTask<Device?> Get(Guid id);

    public ValueTask<PagedList<Device>> Search(DeviceSearchDto dto);

    public ValueTask<Device?> Create(Device request);

    public ValueTask<Device?> Update(Device request);

    public ValueTask<Device?> Delete(Device entity);

    public ValueTask<bool> DeactivebyUser(Guid userId);
}

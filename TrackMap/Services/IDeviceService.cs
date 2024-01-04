using TrackMap.Common.Dtos.Device;
using TrackMap.Common.Requests.Device;
using TrackMap.Common.Responses;
using TrackMap.Common.SeedWork;

namespace TrackMap.Services;

public interface IDeviceService
{
    public ValueTask<List<DeviceResponse>?> GetAll();

    public ValueTask<DeviceResponse?> Get(Guid id);

    public ValueTask<PagedList<DeviceResponse>?> Search(DeviceSearchDto? dto);

    public ValueTask<bool> Create(DeviceCreateRequest? request);

    public ValueTask<bool> Edit(Guid id, DeviceEditRequest? request);

    public ValueTask<bool> Update(Guid id, DeviceUpdateRequest? request);

    public ValueTask<bool> Delete(Guid id);

    public ValueTask<bool> DeactivebyUser(Guid userId);
}

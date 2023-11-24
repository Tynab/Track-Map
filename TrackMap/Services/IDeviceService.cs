using TrackMap.Common.Responses;

namespace TrackMap.Services;

public interface IDeviceService
{
    public ValueTask<List<DeviceResponse>?> GetAll();

    public ValueTask<DeviceResponse?> Get(Guid id);
}

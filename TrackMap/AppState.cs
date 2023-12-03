using TrackMap.Common.Dtos.Device;

namespace TrackMap;

public sealed class AppState
{
    public event Action? OnStateChange;

    public void SetDevicesByUser(List<DeviceDto> devices)
    {
        DevicesByUser = devices ?? [];
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnStateChange?.Invoke();

    public List<DeviceDto>? DevicesByUser { get; set; }
}

using TrackMap.Common.Dtos;

namespace TrackMap;

public sealed class AppState
{
    public event Action? OnStateChange;

    public void SetDevices(List<DeviceDto> devices)
    {
        Devices = devices ?? [];
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnStateChange?.Invoke();

    public List<DeviceDto>? Devices { get; set; }
}

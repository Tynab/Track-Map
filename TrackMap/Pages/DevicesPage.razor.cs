using Microsoft.AspNetCore.Components;
using TrackMap.Common.Dtos;

namespace TrackMap.Pages;

public sealed partial class DevicesPage
{
    protected override void OnInitialized() => Devices = AppState?.Devices;

    [Inject]
    private AppState? AppState { get; set; }

    private List<DeviceDto>? Devices { get; set; }
}

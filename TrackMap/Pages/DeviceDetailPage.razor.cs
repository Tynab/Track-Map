using Microsoft.AspNetCore.Components;
using TrackMap.Common.Dtos.Device;
using YANLib;

namespace TrackMap.Pages;

public sealed partial class DeviceDetailPage
{
    protected override void OnInitialized()
    {
        if (Id.IsNotWhiteSpaceAndNull())
        {
            Device = AppState?.DevicesByUser?.FirstOrDefault(x => x.Id == new Guid(Id));
        }
    }

    [Inject]
    private AppState? AppState { get; set; }

    [Parameter]
    public string? Id { get; set; }

    private DeviceDto? Device { get; set; }
}

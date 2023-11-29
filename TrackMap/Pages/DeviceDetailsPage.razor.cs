using Microsoft.AspNetCore.Components;
using TrackMap.Common.Dtos;
using YANLib;

namespace TrackMap.Pages;

public sealed partial class DeviceDetailsPage
{
    protected override void OnInitialized()
    {
        if (Id!.IsNotWhiteSpaceAndNull())
        {
            Device = AppState?.Devices?.FirstOrDefault(x => x.Id == new Guid(Id));
        }
    }

    [Inject]
    private AppState? AppState { get; set; }

    [Parameter]
    public string? Id { get; set; }

    private DeviceDto? Device { get; set; }
}

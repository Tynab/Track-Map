using BrowserInterop.Extensions;
using BrowserInterop.Geolocation;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using static System.TimeSpan;

namespace TrackMap.Pages;

public sealed partial class IndexPage
{
    protected async override Task OnInitializedAsync()
    {
        Geolocation = (await (await JSRuntime.Window()).Navigator()).Geolocation;
        await GetCurrentPosition();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            Geolocation = (await (await JSRuntime.Window()).Navigator()).Geolocation;
            await GetCurrentPosition();
            await JSRuntime!.InvokeVoidAsync("initMap", Position?.Coords?.Latitude ?? 0, Position?.Coords?.Longitude ?? 0);
            StateHasChanged();
        }
    }

    public async Task GetCurrentPosition() => Position = (await Geolocation!.GetCurrentPosition(new PositionOptions()
    {
        EnableHighAccuracy = true,
        MaximumAgeTimeSpan = FromHours(1),
        TimeoutTimeSpan = FromMinutes(1)
    })).Location;

    [Inject]
    private IJSRuntime? JSRuntime { get; set; }

    private WindowNavigatorGeolocation? Geolocation { get; set; }

    private GeolocationPosition? Position { get; set; }
}

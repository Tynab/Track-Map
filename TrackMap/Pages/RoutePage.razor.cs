using BrowserInterop.Extensions;
using BrowserInterop.Geolocation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using TrackMap.Common.Dtos;
using YANLib;
using static System.Threading.Tasks.Task;
using static System.TimeSpan;

namespace TrackMap.Pages;

public sealed partial class RoutePage : IAsyncDisposable
{
    private const int _timeStep = 1_000 * 60;
    private const double _minDist = 100;

    protected async override Task OnInitializedAsync()
    {
        Wrapper = (await (await JSRuntime.Window()).Navigator()).Geolocation;
        await UpdateLocation();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            Wrapper = (await (await JSRuntime.Window()).Navigator()).Geolocation;
            await UpdateLocation();
            await JSRuntime!.InvokeVoidAsync("initRoute", Route.Latitude, Route.Longitude);
            StateHasChanged();
        }
    }

    private async Task HandleRoute(EditContext context)
    {
        var routeDist = _minDist;

        if (Watcher is null && !IsWatching)
        {
            Watcher = await Wrapper!.WatchPosition(async (p) =>
            {
                IsWatching = true;

                while (routeDist >= _minDist)
                {
                    if (Watcher is null || !IsWatching)
                    {
                        break;
                    }

                    await UpdateLocation();
                    StateHasChanged();
                    routeDist = await JSRuntime!.InvokeAsync<double>("calculateRoute", Route.Latitude, Route.Longitude);
                    await Delay(_timeStep);
                }

                await HandleReset();
            });
        }
    }

    private async Task HandleReset()
    {
        await StopWatch();
        StateHasChanged();
        await JSRuntime!.InvokeVoidAsync("initRoute", Route.Latitude, Route.Longitude);
    }

    private async Task StopWatch()
    {
        IsWatching = false;

        if (Watcher is not null)
        {
            await Watcher.DisposeAsync();
            Watcher = default;
        }
    }

    private async Task GetCurrentPosition() => Position = (await Wrapper!.GetCurrentPosition(new PositionOptions()
    {
        EnableHighAccuracy = true,
        MaximumAgeTimeSpan = FromHours(1),
        TimeoutTimeSpan = FromMinutes(1)
    })).Location;

    private async Task UpdateLocation()
    {
        await GetCurrentPosition();

        var lat = (Position?.Coords?.Latitude ?? 0).ToDecimal();
        var lng = (Position?.Coords?.Longitude ?? 0).ToDecimal();

        Route.Latitude = lat;
        Route.Longitude = lng;
    }

    public async ValueTask DisposeAsync() => await StopWatch();

    [Inject]
    private IJSRuntime? JSRuntime { get; set; }

    private IAsyncDisposable? Watcher { get; set; }

    private WindowNavigatorGeolocation? Wrapper { get; set; }

    private GeolocationPosition? Position { get; set; }

    private RouteDto Route { get; set; } = new RouteDto();

    private bool IsWatching { get; set; } = false;
}

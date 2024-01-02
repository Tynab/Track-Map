using BrowserInterop.Extensions;
using BrowserInterop.Geolocation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using TrackMap.Common.Dtos;
using TrackMap.Layout;
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
        try
        {
            var authenticationState = await AuthenticationState!;

            if (authenticationState.User.Identity is not null && authenticationState.User.Identity.IsAuthenticated)
            {
                Wrapper = (await (await JSRuntime.Window()).Navigator()).Geolocation;
                await UpdateLocation();
            }
        }
        catch (Exception ex)
        {
            Error?.ProcessError(ex);
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        try
        {
            if (firstRender)
            {
                Wrapper = (await (await JSRuntime.Window()).Navigator()).Geolocation;
                await UpdateLocation();
                await JSRuntime.InvokeVoidAsync("initRoute", Route.Latitude, Route.Longitude);
                StateHasChanged();
            }
        }
        catch (Exception ex)
        {
            Error?.ProcessError(ex);
        }
    }

    private async Task HandleRoute(EditContext context)
    {
        try
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
                        routeDist = await JSRuntime.InvokeAsync<double>("calculateRoute", Route.Latitude, Route.Longitude);
                        await Delay(_timeStep);
                    }

                    await HandleReset();
                });
            }
        }
        catch (Exception ex)
        {
            Error?.ProcessError(ex);
        }
    }

    private async Task HandleReset()
    {
        try
        {
            await StopWatch();
            StateHasChanged();
            await JSRuntime.InvokeVoidAsync("initRoute", Route.Latitude, Route.Longitude);
        }
        catch (Exception ex)
        {
            Error?.ProcessError(ex);
        }
    }

    private async Task StopWatch()
    {
        try
        {
            IsWatching = false;

            if (Watcher is not null)
            {
                await Watcher.DisposeAsync();
                Watcher = default;
            }
        }
        catch (Exception ex)
        {
            Error?.ProcessError(ex);
        }
    }

    private async Task GetCurrentPosition()
    {
        try
        {
            Position = (await Wrapper!.GetCurrentPosition(new PositionOptions()
            {
                EnableHighAccuracy = true,
                MaximumAgeTimeSpan = FromHours(1),
                TimeoutTimeSpan = FromMinutes(1)
            })).Location;
        }
        catch (Exception ex)
        {
            Error?.ProcessError(ex);
        }
    }

    private async Task UpdateLocation()
    {
        try
        {
            await GetCurrentPosition();

            var lat = (Position?.Coords?.Latitude ?? 0).ToDecimal();
            var lng = (Position?.Coords?.Longitude ?? 0).ToDecimal();

            Route.Latitude = lat;
            Route.Longitude = lng;
        }
        catch (Exception ex)
        {
            Error?.ProcessError(ex);
        }
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            await StopWatch();
        }
        catch (Exception ex)
        {
            Error?.ProcessError(ex);
        }
    }

    [CascadingParameter]
    private Error? Error { get; set; }

    [CascadingParameter]
    private Task<AuthenticationState>? AuthenticationState { get; set; }

    private IAsyncDisposable? Watcher { get; set; }

    private WindowNavigatorGeolocation? Wrapper { get; set; }

    private GeolocationPosition? Position { get; set; }

    private RouteDto Route { get; set; } = new RouteDto();

    private bool IsWatching { get; set; } = false;
}

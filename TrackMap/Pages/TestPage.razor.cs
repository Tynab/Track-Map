using BrowserInterop.Extensions;
using BrowserInterop.Geolocation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using TrackMap.Common.Dtos;
using TrackMap.Layout;
using static System.Threading.Tasks.Task;

namespace TrackMap.Pages;

public sealed partial class TestPage : IAsyncDisposable
{
    private const int _timeStep = 1_000;
    private const decimal _latSrc = 10.802171230123804m;
    private const decimal _lngSrc = 106.6449702965176m;
    private const decimal _latDest = 10.802530627366744m;
    private const decimal _lngDest = 106.64076701888838m;
    private const decimal _latStep = .000005989954048m;
    private const decimal _lngStep = .00007005462715m;
    private readonly decimal _latLastStep = _latDest - _latStep * 2;
    private readonly decimal _lngLastStep = _lngDest - _lngStep * 2;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            var authenticationState = await AuthenticationState!;

            if (authenticationState.User.Identity is not null && authenticationState.User.Identity.IsAuthenticated)
            {
                Wrapper = (await (await JSRuntime.Window()).Navigator()).Geolocation;
                Route.Latitude = _latSrc;
                Route.Longitude = _lngSrc;
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
                await JSRuntime.InvokeVoidAsync("initRoute", _latSrc, _lngSrc);
                StateHasChanged();
            }
        }
        catch (Exception ex)
        {
            Error?.ProcessError(ex);
        }
    }

    private async Task HandleWatch()
    {
        try
        {
            if (Watcher is null && !IsWatching)
            {
                Watcher = await Wrapper!.WatchPosition(async (p) =>
                {
                    IsWatching = true;

                    while (Route.Latitude < _latLastStep && Route.Longitude > _lngLastStep)
                    {
                        if (Watcher is null || !IsWatching)
                        {
                            break;
                        }

                        if (Route.Latitude < _latLastStep)
                        {
                            Route.Latitude += _latStep;
                        }

                        if (Route.Longitude > _lngLastStep)
                        {
                            Route.Longitude -= _lngStep;
                        }

                        StateHasChanged();
                        await JSRuntime.InvokeVoidAsync("calculateRoute", Route.Latitude, Route.Longitude);
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
    private Task<AuthenticationState>? AuthenticationState { get; set; }

    [CascadingParameter]
    private Error? Error { get; set; }

    private IAsyncDisposable? Watcher { get; set; }

    private WindowNavigatorGeolocation? Wrapper { get; set; }

    private RouteDto Route { get; set; } = new RouteDto();

    private bool IsWatching { get; set; } = false;
}

using BrowserInterop.Extensions;
using BrowserInterop.Geolocation;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TrackMap.Common.Dtos;
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
        Wrapper = (await (await JSRuntime.Window()).Navigator()).Geolocation;
        Route.Latitude = _latSrc;
        Route.Longitude = _lngSrc;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JSRuntime!.InvokeVoidAsync("initRoute", _latSrc, _lngSrc);
            StateHasChanged();
        }
    }

    private async Task HandleWatch()
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
                    await JSRuntime!.InvokeVoidAsync("calculateRoute", Route.Latitude, Route.Longitude);
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

    public async ValueTask DisposeAsync() => await StopWatch();

    [Inject]
    private IJSRuntime? JSRuntime { get; set; }

    private IAsyncDisposable? Watcher { get; set; }

    private WindowNavigatorGeolocation? Wrapper { get; set; }

    private RouteDto Route { get; set; } = new RouteDto();

    private bool IsWatching { get; set; } = false;
}

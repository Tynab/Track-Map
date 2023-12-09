using BrowserInterop.Extensions;
using BrowserInterop.Geolocation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using TrackMap.Common.Dtos;
using TrackMap.Common.Responses;
using static System.TimeSpan;

namespace TrackMap.Pages;

public sealed partial class WaypointsPage
{
    private List<DeviceResponse>? _devices;

    protected async override Task OnInitializedAsync()
    {
        var authenticationState = await AuthenticationState!;

        if (authenticationState.User.Identity is not null && authenticationState.User.Identity.IsAuthenticated)
        {
            _devices = await DeviceService.GetAll();
            Geolocation = (await (await JSRuntime.Window()).Navigator()).Geolocation;
            await GetCurrentPosition();
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            Geolocation = (await (await JSRuntime.Window()).Navigator()).Geolocation;
            await GetCurrentPosition();
            await JSRuntime.InvokeVoidAsync("initWaypoints", Position?.Coords?.Latitude ?? 0, Position?.Coords?.Longitude ?? 0);
            StateHasChanged();
        }
    }

    private async Task HandleWaypoints(EditContext context) => await JSRuntime.InvokeVoidAsync("calculateWaypoints", _devices?.Select(x => new
    {
        location = $"{x.Latitude},{x.Longitude}",
        stopover = true
    }));

    public async Task GetCurrentPosition() => Position = (await Geolocation!.GetCurrentPosition(new PositionOptions()
    {
        EnableHighAccuracy = true,
        MaximumAgeTimeSpan = FromHours(1),
        TimeoutTimeSpan = FromMinutes(1)
    })).Location;

    [CascadingParameter]
    private Task<AuthenticationState>? AuthenticationState { get; set; }

    private WindowNavigatorGeolocation? Geolocation { get; set; }

    private GeolocationPosition? Position { get; set; }

    private DirectionDto? Waypoint { get; set; } = new DirectionDto();
}

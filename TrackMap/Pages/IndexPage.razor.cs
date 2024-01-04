using BrowserInterop.Extensions;
using BrowserInterop.Geolocation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using TrackMap.Common.Dtos.Device;
using TrackMap.Common.Enums;
using TrackMap.Common.Requests.Device;
using TrackMap.Common.Responses;
using TrackMap.Common.Utilities;
using TrackMap.Layout;
using YANLib;
using static System.Threading.Tasks.Task;
using static System.TimeSpan;
using static TrackMap.Common.Enums.Status;

namespace TrackMap.Pages;

public sealed partial class IndexPage
{
    protected async override Task OnInitializedAsync()
    {
        try
        {
            var authenticationState = await AuthenticationState!;

            if (authenticationState.User.Identity is not null && authenticationState.User.Identity.IsAuthenticated)
            {
                Geolocation = (await (await JSRuntime.Window()).Navigator()).Geolocation;

                var ipTask = JSRuntime.InvokeAsync<string>("getIpAddress").AsTask();
                var userTask = LocalStorageService.GetItemAsync<UserResponse>("profile").AsTask();

                await WhenAll(ipTask, userTask);

                var ip = await ipTask;
                var user = await userTask;

                var devsTask = DeviceService.Search(new DeviceSearchDto
                {
                    UserId = user.Id,
                    IpAddress = ip
                }).AsTask();

                var agtTask = JSRuntime.InvokeAsync<string>("getUserAgent").AsTask();
                var posTask = GetCurrentPosition();
                var deactiveTask = DeviceService.DeactivebyUser(user.Id).AsTask();

                await WhenAll(devsTask, agtTask, posTask, deactiveTask);

                var os = (await agtTask).CheckOs();
                var existDev = (await devsTask)?.Items?.FirstOrDefault();

                _ = existDev is null
                    ? await DeviceService.Create(new DeviceCreateRequest
                    {
                        DeviceType = ((DeviceOs?)os).CheckType(),
                        DeviceOs = os,
                        IpAddress = ip,
                        Latitude = (Position?.Coords?.Latitude ?? 0).ToDecimal(),
                        Longitude = (Position?.Coords?.Longitude ?? 0).ToDecimal(),
                        UserId = user.Id,
                        CreatedBy = user.Id
                    })
                    : await DeviceService.Update(existDev.Id, new DeviceUpdateRequest
                    {
                        DeviceType = ((DeviceOs?)os).CheckType(),
                        DeviceOs = os,
                        IpAddress = ip,
                        Latitude = (Position?.Coords?.Latitude ?? 0).ToDecimal(),
                        Longitude = (Position?.Coords?.Longitude ?? 0).ToDecimal(),
                        UpdatedBy = user.Id,
                        Status = Active
                    });
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
                Geolocation = (await (await JSRuntime.Window()).Navigator()).Geolocation;
                await GetCurrentPosition();
                await JSRuntime.InvokeVoidAsync("initMap", Position?.Coords?.Latitude ?? 0, Position?.Coords?.Longitude ?? 0);
                StateHasChanged();
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
            Position = (await Geolocation!.GetCurrentPosition(new PositionOptions()
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

    [CascadingParameter]
    private Task<AuthenticationState>? AuthenticationState { get; set; }

    [CascadingParameter]
    private Error? Error { get; set; }

    private WindowNavigatorGeolocation? Geolocation { get; set; }

    private GeolocationPosition? Position { get; set; }
}

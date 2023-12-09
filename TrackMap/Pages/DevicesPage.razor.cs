﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using TrackMap.Common.Dtos.Device;
using TrackMap.Common.Responses;
using static System.Threading.Tasks.Task;

namespace TrackMap.Pages;

public sealed partial class DevicesPage
{
    protected override async Task OnInitializedAsync()
    {
        var authenticationState = await AuthenticationState!;

        if (authenticationState.User.Identity is not null && authenticationState.User.Identity.IsAuthenticated)
        {
            var usersTask = UserService.GetAll().AsTask();
            var devsTask = DeviceService.GetAll().AsTask();

            await WhenAll(usersTask, devsTask);
            Users = await usersTask;
            Devices = await devsTask;
        }
    }

    private async Task HandleDeviceSearch(EditContext context)
    {
        Devices = await DeviceService.Search(DeviceSearch);
        ToastService.ShowInfo("Seach completed");
    }

    [CascadingParameter]
    private Task<AuthenticationState>? AuthenticationState { get; set; }

    private List<DeviceResponse>? Devices { get; set; }

    private List<UserResponse>? Users { get; set; }

    private DeviceSearchDto DeviceSearch { get; set; } = new DeviceSearchDto();
}
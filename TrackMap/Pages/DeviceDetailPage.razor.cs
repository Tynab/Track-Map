﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using TrackMap.Common.Responses;
using YANLib;

namespace TrackMap.Pages;

public sealed partial class DeviceDetailPage
{
    protected override async Task OnInitializedAsync()
    {
        var authenticationState = await AuthenticationState!;

        if (authenticationState.User.Identity is not null && authenticationState.User.Identity.IsAuthenticated && Id.IsNotWhiteSpaceAndNull())
        {
            Device = await DeviceService.Get(new Guid(Id));
        }
    }

    [Parameter]
    public string? Id { get; set; }

    [CascadingParameter]
    private Task<AuthenticationState>? AuthenticationState { get; set; }

    private DeviceResponse? Device { get; set; }
}

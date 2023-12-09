using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using TrackMap.Common.Dtos.Device;
using TrackMap.Common.Responses;
using YANLib;

namespace TrackMap.Pages;

public sealed partial class UserDevicesPage
{
    protected override async Task OnInitializedAsync()
    {
        var authenticationState = await AuthenticationState!;

        if (authenticationState.User.Identity is not null && authenticationState.User.Identity.IsAuthenticated)
        {
            Users = await UserService.GetAll().AsTask();

            if (Id.IsNotWhiteSpaceAndNull())
            {
                Devices = await DeviceService.Search(new DeviceSearchDto
                {
                    UserId = new Guid(Id)
                });
            }
        }
    }

    private async Task HandleDeviceSearch(EditContext context)
    {
        Devices = await DeviceService.Search(DeviceSearch);
        ToastService.ShowInfo("Seach completed");
    }

    [Parameter]
    public string? Id { get; set; }

    [CascadingParameter]
    private Task<AuthenticationState>? AuthenticationState { get; set; }

    private List<DeviceResponse>? Devices { get; set; }

    private List<UserResponse>? Users { get; set; }

    private DeviceSearchDto DeviceSearch { get; set; } = new DeviceSearchDto();
}

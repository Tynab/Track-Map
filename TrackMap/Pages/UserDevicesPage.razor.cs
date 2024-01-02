using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using TrackMap.Common.Dtos.Device;
using TrackMap.Common.Responses;
using TrackMap.Layout;
using YANLib;

namespace TrackMap.Pages;

public sealed partial class UserDevicesPage
{
    protected override async Task OnInitializedAsync()
    {
        try
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
        catch (Exception ex)
        {
            Error?.ProcessError(ex);
        }
    }

    private async Task HandleDeviceSearch(EditContext context)
    {
        try
        {
            Devices = await DeviceService.Search(DeviceSearch);
            ToastService.ShowInfo("Seach completed");
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

    [Parameter]
    public string? Id { get; set; }

    private List<DeviceResponse>? Devices { get; set; }

    private List<UserResponse>? Users { get; set; }

    private DeviceSearchDto DeviceSearch { get; set; } = new DeviceSearchDto();
}

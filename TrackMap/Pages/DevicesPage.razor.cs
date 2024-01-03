using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using TrackMap.Common.Dtos.Device;
using TrackMap.Common.Responses;
using TrackMap.Components;
using TrackMap.Layout;
using static System.Threading.Tasks.Task;

namespace TrackMap.Pages;

public sealed partial class DevicesPage
{
    private Guid _deleteId;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            var authenticationState = await AuthenticationState!;

            if (authenticationState.User.Identity is not null && authenticationState.User.Identity.IsAuthenticated)
            {
                var userTask = LocalStorageService.GetItemAsync<UserResponse>("profile").AsTask();
                var usersTask = UserService.GetAll().AsTask();
                var devsTask = DeviceService.GetAll().AsTask();

                await WhenAll(userTask, usersTask, devsTask);
                User = await userTask;
                Users = await usersTask;
                Devices = await devsTask;

                if (authenticationState.User.IsInRole("Admin"))
                {
                    IsAdmin = true;
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

    public void OnDeleteDevice(Guid id)
    {
        try
        {
            _deleteId = id;
            DeleteConfirmation?.Show();
        }
        catch (Exception ex)
        {
            Error?.ProcessError(ex);
        }
    }

    public async Task OnConfirmDeleteDevice(bool isConfirmed)
    {
        try
        {
            if (isConfirmed && await DeviceService.Delete(_deleteId))
            {
                ToastService.ShowSuccess("Delete successful");
                Devices = await DeviceService.Search(DeviceSearch);
            }
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

    private List<DeviceResponse>? Devices { get; set; }

    private List<UserResponse>? Users { get; set; }

    private Confirmation? DeleteConfirmation { get; set; }

    private UserResponse? User { get; set; }

    private DeviceSearchDto DeviceSearch { get; set; } = new DeviceSearchDto();

    private bool IsAdmin { get; set; } = false;
}

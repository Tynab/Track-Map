using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using TrackMap.Common.Dtos.Device;
using TrackMap.Common.Responses;
using TrackMap.Common.SeedWork;
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

                await WhenAll(userTask, usersTask, GetDevices().AsTask());
                User = await userTask;
                Users = await usersTask;

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
            if (await GetDevices())
            {
                ToastService.ShowInfo("Seach completed");
            }
        }
        catch (Exception ex)
        {
            Error?.ProcessError(ex);
        }
    }

    private void OnDeleteDevice(Guid id)
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

    private async Task OnConfirmDeleteDevice(bool isConfirmed)
    {
        try
        {
            if (isConfirmed && await DeviceService.Delete(_deleteId))
            {
                if (await GetDevices())
                {
                    ToastService.ShowSuccess("Delete successful");
                }
            }
        }
        catch (Exception ex)
        {
            Error?.ProcessError(ex);
        }
    }

    private async Task SelectedPage(int page)
    {
        try
        {
            DeviceSearch.PageNumber = page;
            _ = await GetDevices();
        }
        catch (Exception ex)
        {
            Error?.ProcessError(ex);
        }
    }

    private async ValueTask<bool> GetDevices()
    {
        try
        {
            var pagingRes = await DeviceService.Search(DeviceSearch);

            if (pagingRes is not null && pagingRes.MetaData is not null)
            {
                Devices = pagingRes.Items;
                MetaData = pagingRes.MetaData;
            }

            return true;
        }
        catch (Exception ex)
        {
            Error?.ProcessError(ex);

            return false;
        }
    }

    [CascadingParameter]
    private Task<AuthenticationState>? AuthenticationState { get; set; }

    [CascadingParameter]
    private Error? Error { get; set; }

    private List<DeviceResponse>? Devices { get; set; }

    private List<UserResponse>? Users { get; set; }

    private Confirmation? DeleteConfirmation { get; set; }

    private UserResponse? User { get; set; }

    private DeviceSearchDto DeviceSearch { get; set; } = new DeviceSearchDto();

    private MetaData MetaData { get; set; } = new MetaData();

    private bool IsAdmin { get; set; } = false;
}

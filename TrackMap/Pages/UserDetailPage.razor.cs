using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using TrackMap.Common.Dtos.Device;
using TrackMap.Common.Responses;
using TrackMap.Common.SeedWork;
using TrackMap.Components;
using TrackMap.Layout;
using YANLib;

namespace TrackMap.Pages;

public sealed partial class UserDetailPage
{
    private Guid _deleteId;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            var authenticationState = await AuthenticationState!;

            if (authenticationState.User.Identity is not null && authenticationState.User.Identity.IsAuthenticated && Id.IsNotWhiteSpaceAndNull())
            {
                User = await UserService.Get(new Guid(Id)).AsTask();
                _ = await GetDevicesByUser();
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
            if (await GetDevicesByUser())
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

    private async Task OnConfirmDeleteDevice(bool isComfirmed)
    {
        try
        {
            if (isComfirmed && await DeviceService.Delete(_deleteId))
            {
                if (await GetDevicesByUser())
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
            _ = await GetDevicesByUser();
        }
        catch (Exception ex)
        {
            Error?.ProcessError(ex);
        }
    }

    private async ValueTask<bool> GetDevicesByUser()
    {
        try
        {
            DeviceSearch.UserId = User?.Id;

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

    [Parameter]
    public string? Id { get; set; }

    [CascadingParameter]
    private Task<AuthenticationState>? AuthenticationState { get; set; }

    [CascadingParameter]
    private Error? Error { get; set; }

    private List<DeviceResponse>? Devices { get; set; }

    private Confirmation? DeleteConfirmation { get; set; }

    private UserResponse? User { get; set; }

    private DeviceSearchDto DeviceSearch { get; set; } = new DeviceSearchDto();

    private MetaData MetaData { get; set; } = new MetaData();
}

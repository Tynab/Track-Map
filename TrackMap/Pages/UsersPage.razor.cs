using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using TrackMap.Common.Dtos.Device;
using TrackMap.Common.Dtos.User;
using TrackMap.Common.Responses;
using TrackMap.Services;

namespace TrackMap.Pages;

public sealed partial class UsersPage
{
    protected override async Task OnInitializedAsync()
    {
        Users = await UserService!.GetAll();
        AppState!.OnStateChange += StateHasChanged;
    }

    private async Task HandleUserSearch(EditContext context)
    {
        ToastService!.ShowInfo("Seach completed");
        Users = await UserService!.Search(UserSearch);
    }

    private void HandleDevices(List<DeviceDto> devices) => AppState?.SetDevicesByUser(devices);

    public void Dispose() => AppState!.OnStateChange -= StateHasChanged;

    [Inject]
    private IToastService? ToastService { get; set; }

    [Inject]
    private IUserService? UserService { get; set; }

    [Inject]
    private AppState? AppState { get; set; }

    private List<UserResponse>? Users { get; set; }

    private UserSearchDto UserSearch { get; set; } = new UserSearchDto();
}

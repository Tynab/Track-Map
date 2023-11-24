using Microsoft.AspNetCore.Components;
using TrackMap.Common.Dtos;
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

    private void HandleDevices(List<DeviceDto> devices) => AppState?.SetDevices(devices);

    public void Dispose() => AppState!.OnStateChange -= StateHasChanged;

    [Inject]
    private IUserService? UserService { get; set; }

    [Inject]
    private AppState? AppState { get; set; }

    private List<UserResponse>? Users { get; set; }
}

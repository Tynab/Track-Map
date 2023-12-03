using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using TrackMap.Common.Dtos.Device;
using TrackMap.Common.Responses;
using TrackMap.Services;

namespace TrackMap.Pages;

public sealed partial class DevicesPage
{
    protected override void OnInitialized() => DevicesByUser = AppState?.DevicesByUser;

    protected override async Task OnInitializedAsync()
    {
        Devices = await DeviceService!.GetAll();
        Users = await UserService!.GetAll();
    }

    private async Task HandleDeviceSearch(EditContext context)
    {
        ToastService!.ShowInfo("Seach completed");
        Devices = await DeviceService!.Search(DeviceSearch);
    }

    [Inject]
    private IToastService? ToastService { get; set; }

    [Inject]
    private IDeviceService? DeviceService { get; set; }

    [Inject]
    private IUserService? UserService { get; set; }

    [Inject]
    private AppState? AppState { get; set; }

    private List<DeviceDto>? DevicesByUser { get; set; }

    private List<DeviceResponse>? Devices { get; set; }

    private List<UserResponse>? Users { get; set; }

    private DeviceSearchDto DeviceSearch { get; set; } = new DeviceSearchDto();
}

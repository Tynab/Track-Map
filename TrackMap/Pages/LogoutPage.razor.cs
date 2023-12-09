using Microsoft.AspNetCore.Components;
using static System.Threading.Tasks.Task;

namespace TrackMap.Pages;

public sealed partial class LogoutPage
{
    protected override async Task OnInitializedAsync()
    {
        var clrTask = LocalStorageService.RemoveItemAsync("profile").AsTask();
        var outTask = AuthService.Logout().AsTask();

        await WhenAll(clrTask, outTask);
        ToastService.ShowSuccess("Logout successful");
        NavigationManager.NavigateTo("/login");
    }
}

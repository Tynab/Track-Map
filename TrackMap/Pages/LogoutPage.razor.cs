using Microsoft.AspNetCore.Components;
using TrackMap.Layout;
using static System.Threading.Tasks.Task;

namespace TrackMap.Pages;

public sealed partial class LogoutPage
{
    protected override async Task OnInitializedAsync()
    {
        try
        {
            var clrTask = LocalStorageService.RemoveItemAsync("profile").AsTask();
            var outTask = AuthService.Logout().AsTask();

            await WhenAll(clrTask, outTask);
            ToastService.ShowSuccess("Logout successful");
            NavigationManager.NavigateTo("/login");
        }
        catch (Exception ex)
        {
            Error?.ProcessError(ex);
        }
    }

    [CascadingParameter]
    private Error? Error { get; set; }
}

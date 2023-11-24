using Microsoft.AspNetCore.Components;
using TrackMap.Services;

namespace TrackMap.Pages;

public sealed partial class LogoutPage
{
    protected override async Task OnInitializedAsync()
    {
        await AuthService!.Logout();
        NavigationManager?.NavigateTo("/");
    }

    [Inject]
    private NavigationManager? NavigationManager { get; set; }

    [Inject]
    private IAuthService? AuthService { get; set; }
}

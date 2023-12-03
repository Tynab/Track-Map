using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using TrackMap.Common.Requests;
using TrackMap.Services;

namespace TrackMap.Pages;

public sealed partial class LoginPage
{
    private async Task HandleLogin()
    {
        ShowErrors = default;

        var rslt = await AuthService!.Login(Login);

        if (rslt is null)
        {
            ShowErrors = true;
            Error = "Not responding";
        }
        else
        {
            if (rslt.Success)
            {
                ToastService!.ShowSuccess("Login successful");
                NavigationManager?.NavigateTo("/");
            }
            else
            {
                ShowErrors = true;
                Error = rslt.Error ?? "Not responding";
            }
        }
    }

    [Inject]
    private NavigationManager? NavigationManager { get; set; }

    [Inject]
    private IToastService? ToastService { get; set; }

    [Inject]
    private IAuthService? AuthService { get; set; }

    private LoginRequest Login { get; set; } = new LoginRequest();

    private bool ShowErrors { get; set; }

    private string Error { get; set; } = string.Empty;
}

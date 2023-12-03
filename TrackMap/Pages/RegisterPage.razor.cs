using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using TrackMap.Common.Requests;
using TrackMap.Services;

namespace TrackMap.Pages;

public sealed partial class RegisterPage
{
    private async Task HandleRegister()
    {
        ShowErrors = default;

        var rslt = await AuthService!.Register(Register);

        if (rslt is null)
        {
            ShowErrors = true;
            Errors = new string[]
            {
                "Not responding"
            };
        }
        else
        {
            if (rslt.Success)
            {
                ToastService!.ShowSuccess("Registration successful");
                NavigationManager?.NavigateTo("/login");
            }
            else
            {
                ShowErrors = true;
                Errors = rslt.Errors ?? new string[]
                {
                    "Not responding"
                };
            }
        }
    }

    [Inject]
    private NavigationManager? NavigationManager { get; set; }

    [Inject]
    private IToastService? ToastService { get; set; }

    [Inject]
    private IAuthService? AuthService { get; set; }

    private bool ShowErrors { get; set; }

    private IEnumerable<string>? Errors { get; set; } = default;

    private RegisterRequest Register { get; set; } = new RegisterRequest();
}

using Microsoft.AspNetCore.Components;
using TrackMap.Common.Requests;
using TrackMap.Layout;

namespace TrackMap.Pages;

public sealed partial class RegisterPage
{
    private async Task HandleRegister()
    {
        try
        {
            ShowErrors = default;

            var rslt = await AuthService.Register(Register);

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
                    ToastService.ShowSuccess("Registration successful");
                    NavigationManager.NavigateTo("/login");
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
        catch (Exception ex)
        {
            Error?.ProcessError(ex);
        }
    }

    [CascadingParameter]
    private Error? Error { get; set; }

    private IEnumerable<string>? Errors { get; set; }

    private bool ShowErrors { get; set; }

    private RegisterRequest Register { get; set; } = new RegisterRequest();
}

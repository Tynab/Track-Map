using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using TrackMap.Common.Dtos.User;
using TrackMap.Common.Requests;
using TrackMap.Layout;
using YANLib;

namespace TrackMap.Pages;

public sealed partial class LoginPage
{
    private async Task HandleLogin()
    {
        try
        {
            ShowErrors = default;

            var rslt = await AuthService.Login(Login);

            if (rslt is null)
            {
                ShowErrors = true;
                ErrorMessage = "Not responding";
            }
            else
            {
                if (rslt.Success)
                {
                    var authenticationState = await AuthenticationState!;

                    if (authenticationState.User.Identity is not null && authenticationState.User.Identity.Name.IsNotWhiteSpaceAndNull())
                    {
                        await LocalStorageService.SetItemAsync("profile", (await UserService.Search(new UserSearchDto
                        {
                            UserName = authenticationState.User.Identity.Name
                        }))?.FirstOrDefault());
                    }

                    ToastService.ShowSuccess("Login successful");
                    NavigationManager.NavigateTo("/");
                }
                else
                {
                    ShowErrors = true;
                    ErrorMessage = rslt.Error ?? "Not responding";
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

    [CascadingParameter]
    private Task<AuthenticationState>? AuthenticationState { get; set; }

    private bool ShowErrors { get; set; }

    private string ErrorMessage { get; set; } = string.Empty;

    private LoginRequest Login { get; set; } = new LoginRequest();
}

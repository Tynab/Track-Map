using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using TrackMap.Common.Requests.User;
using TrackMap.Common.Responses;
using TrackMap.Layout;

namespace TrackMap.Pages;

public sealed partial class ProfilePage
{
    protected override async Task OnInitializedAsync()
    {
        try
        {
            var authenticationState = await AuthenticationState!;

            if (authenticationState.User.Identity is not null && authenticationState.User.Identity.IsAuthenticated)
            {
                User = await LocalStorageService.GetItemAsync<UserResponse>("profile");
                UserUpdate.FullName = User.FullName;
                UserUpdate.Email = User.Email;
                UserUpdate.PhoneNumber = User.PhoneNumber;
                UserUpdate.UpdatedBy = User.Id;
            }
        }
        catch (Exception ex)
        {
            Error?.ProcessError(ex);
        }
    }

    private async Task HandleUserUpdate()
    {
        try
        {
            if (User is not null && await UserService.Update(User.Id, UserUpdate))
            {
                await LocalStorageService.SetItemAsync("profile", await UserService.Get(User.Id));
                ToastService.ShowSuccess("Update successful");
            }
            else
            {
                ToastService.ShowError("An error occurred in progress");
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

    private UserResponse? User { get; set; }

    private UserUpdateRequest UserUpdate { get; set; } = new UserUpdateRequest();
}

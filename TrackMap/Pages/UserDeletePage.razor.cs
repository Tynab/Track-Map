using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using YANLib;

namespace TrackMap.Pages;

public sealed partial class UserDeletePage
{
    protected override async Task OnInitializedAsync()
    {
        var authenticationState = await AuthenticationState!;

        if (authenticationState.User.Identity is not null && authenticationState.User.Identity.IsAuthenticated && Id.IsNotWhiteSpaceAndNull())
        {
            Result = await UserService.Delete(new Guid(Id));

            if (Result.Value)
            {
                ToastService.ShowSuccess("Delete successful");
            }
            else
            {
                ToastService.ShowError("An error occurred in progress");
            }

            NavigationManager.NavigateTo("/users");
        }
    }

    [Parameter]
    public string? Id { get; set; }

    [CascadingParameter]
    private Task<AuthenticationState>? AuthenticationState { get; set; }

    private bool? Result { get; set; } = null;
}

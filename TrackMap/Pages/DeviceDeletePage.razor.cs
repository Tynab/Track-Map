using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using YANLib;

namespace TrackMap.Pages;

public sealed partial class DeviceDeletePage
{
    protected override async Task OnInitializedAsync()
    {
        var authenticationState = await AuthenticationState!;

        if (authenticationState.User.Identity is not null && authenticationState.User.Identity.IsAuthenticated && Id.IsNotWhiteSpaceAndNull())
        {
            Result = await DeviceService.Delete(new Guid(Id));

            if (Result.Value)
            {
                ToastService.ShowSuccess("Delete successful");
            }
            else
            {
                ToastService.ShowError("An error occurred in progress");
            }

            NavigationManager.NavigateTo("/devices");
        }
    }

    [Parameter]
    public string? Id { get; set; }

    [CascadingParameter]
    private Task<AuthenticationState>? AuthenticationState { get; set; }

    private bool? Result { get; set; } = null;
}

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using TrackMap.Layout;
using YANLib;

namespace TrackMap.Pages;

public sealed partial class DeviceDeletePage
{
    protected override async Task OnInitializedAsync()
    {
        try
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
        catch (Exception ex)
        {
            Error?.ProcessError(ex);
        }
    }

    [CascadingParameter]
    private Error? Error { get; set; }

    [CascadingParameter]
    private Task<AuthenticationState>? AuthenticationState { get; set; }

    [Parameter]
    public string? Id { get; set; }

    private bool? Result { get; set; } = null;
}

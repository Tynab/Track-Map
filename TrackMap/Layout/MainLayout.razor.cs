using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using TrackMap.Common.Responses;
using static BlazorBootstrap.IconName;
using static Microsoft.AspNetCore.Components.Routing.NavLinkMatch;
using static System.Threading.Tasks.Task;

namespace TrackMap.Layout;

public sealed partial class MainLayout
{
    protected override async Task OnInitializedAsync()
    {
        var authenticationState = await AuthenticationState!;

        if (authenticationState.User.Identity is not null && authenticationState.User.Identity.IsAuthenticated)
        {
            User = await LocalStorageService.GetItemAsync<UserResponse>("profile");

            if (User is null)
            {
                ToastService.ShowError("An error occurred in progress");
            }
        }
    }

    private static async Task<SidebarDataProviderResult> SidebarDataProvider(SidebarDataProviderRequest request) => await FromResult(request.ApplyTo(new List<NavItem>
    {
        new() { Href = "/", IconName = HouseDoorFill, Text = "Home", Match=All},
        new() { Href = "/direction", IconName = GeoFill, Text = "Direction"},
        new() { Href = "/route", IconName = PinMapFill, Text = "Route"},
        new() { Href = "/waypoints", IconName = GeoAltFill, Text = "Waypoints"},
        new() { Href = "/profile", IconName = PersonFill, Text = "Profile"},
        new() { Href = "/users", IconName = PeopleFill, Text = "Users"},
        new() { Href = "/devices", IconName = CpuFill, Text = "Devices"},
        new() { Href = "/test", IconName = UiChecksGrid, Text = "Test"},
    }));

    [CascadingParameter]
    private Task<AuthenticationState>? AuthenticationState { get; set; }

    private UserResponse? User { get; set; }
}

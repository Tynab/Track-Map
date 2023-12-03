namespace TrackMap.Shared;

public sealed partial class NavMenu
{
    private bool CollapseNavMenu = true;

    private string? NavMenuCssClass => CollapseNavMenu ? "collapse" : default;

    private void ToggleNavMenu() => CollapseNavMenu = !CollapseNavMenu;
}

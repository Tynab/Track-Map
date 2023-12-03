using Microsoft.AspNetCore.Components;
using TrackMap.Common.Responses;
using TrackMap.Services;
using YANLib;

namespace TrackMap.Pages;

public sealed partial class UserDetailPage
{
    protected override async Task OnInitializedAsync()
    {
        if (Id.IsNotWhiteSpaceAndNull())
        {
            User = await UserService!.Get(new Guid(Id));
        }
    }

    [Parameter]
    public string? Id { get; set; }

    [Inject]
    private IUserService? UserService { get; set; }

    private UserResponse? User { get; set; }
}

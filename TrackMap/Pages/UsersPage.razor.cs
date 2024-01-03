using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using TrackMap.Common.Dtos.User;
using TrackMap.Common.Responses;
using TrackMap.Components;
using TrackMap.Layout;
using static System.Threading.Tasks.Task;

namespace TrackMap.Pages;

public sealed partial class UsersPage
{
    private Guid _deleteId;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            var authenticationState = await AuthenticationState!;

            if (authenticationState.User.Identity is not null && authenticationState.User.Identity.IsAuthenticated)
            {
                var userTask = LocalStorageService.GetItemAsync<UserResponse>("profile").AsTask();
                var usersTask = UserService.GetAll().AsTask();

                await WhenAll(userTask, usersTask);
                User = await userTask;
                Users = await usersTask;

                if (authenticationState.User.IsInRole("Admin"))
                {
                    IsAdmin = true;
                }
            }
        }
        catch (Exception ex)
        {
            Error?.ProcessError(ex);
        }
    }

    private async Task HandleUserSearch(EditContext context)
    {
        try
        {
            Users = await UserService.Search(UserSearch);
            ToastService.ShowInfo("Seach completed");
        }
        catch (Exception ex)
        {
            Error?.ProcessError(ex);
        }
    }

    public void OnDeleteUser(Guid id)
    {
        try
        {
            _deleteId = id;
            DeleteConfirmation?.Show();
        }
        catch (Exception ex)
        {
            Error?.ProcessError(ex);
        }
    }

    public async Task OnConfirmDeleteUser(bool isConfirmed)
    {
        try
        {
            if (isConfirmed && await UserService.Delete(_deleteId))
            {
                ToastService.ShowSuccess("Delete successful");
                Users = await UserService.Search(UserSearch);
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

    private List<UserResponse>? Users { get; set; }

    private Confirmation? DeleteConfirmation { get; set; }

    private UserResponse? User { get; set; }

    private UserSearchDto UserSearch { get; set; } = new UserSearchDto();

    private bool IsAdmin { get; set; } = false;
}

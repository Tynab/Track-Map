using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using TrackMap.Common.Dtos.User;
using TrackMap.Common.Responses;
using TrackMap.Common.SeedWork;
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

                await WhenAll(userTask, GetUsers().AsTask());
                User = await userTask;

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
            if (await GetUsers())
            {
                ToastService.ShowInfo("Seach completed");
            }
        }
        catch (Exception ex)
        {
            Error?.ProcessError(ex);
        }
    }

    private void OnDeleteUser(Guid id)
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

    private async Task OnConfirmDeleteUser(bool isConfirmed)
    {
        try
        {
            if (isConfirmed && await UserService.Delete(_deleteId))
            {
                if (await GetUsers())
                {
                    ToastService.ShowSuccess("Delete successful");
                }
            }
        }
        catch (Exception ex)
        {
            Error?.ProcessError(ex);
        }
    }

    private async Task SelectedPage(int page)
    {
        try
        {
            UserSearch.PageNumber = page;
            _ = await GetUsers();
        }
        catch (Exception ex)
        {
            Error?.ProcessError(ex);
        }
    }

    private async ValueTask<bool> GetUsers()
    {
        try
        {
            var pagingRes = await UserService.Search(UserSearch);

            if (pagingRes is not null && pagingRes.MetaData is not null)
            {
                Users = pagingRes.Items;
                MetaData = pagingRes.MetaData;
            }

            return true;
        }
        catch (Exception ex)
        {
            Error?.ProcessError(ex);

            return false;
        }
    }

    [CascadingParameter]
    private Task<AuthenticationState>? AuthenticationState { get; set; }

    [CascadingParameter]
    private Error? Error { get; set; }

    private List<UserResponse>? Users { get; set; }

    private Confirmation? DeleteConfirmation { get; set; }

    private UserResponse? User { get; set; }

    private UserSearchDto UserSearch { get; set; } = new UserSearchDto();

    private MetaData MetaData { get; set; } = new MetaData();

    private bool IsAdmin { get; set; } = false;
}

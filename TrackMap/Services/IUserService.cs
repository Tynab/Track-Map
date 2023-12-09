using TrackMap.Common.Dtos.User;
using TrackMap.Common.Requests.User;
using TrackMap.Common.Responses;

namespace TrackMap.Services;

public interface IUserService
{
    public ValueTask<List<UserResponse>?> GetAll();

    public ValueTask<UserResponse?> Get(Guid id);

    public ValueTask<List<UserResponse>?> Search(UserSearchDto? dto);

    public ValueTask<bool> Create(UserCreateRequest? request);

    public ValueTask<bool> Edit(Guid id, UserEditRequest? request);

    public ValueTask<bool> Update(Guid id, UserUpdateRequest? request);

    public ValueTask<bool> Delete(Guid id);
}

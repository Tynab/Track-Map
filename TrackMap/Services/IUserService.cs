using TrackMap.Common.Responses;

namespace TrackMap.Services;

public interface IUserService
{
    public ValueTask<List<UserResponse>?> GetAll();

    public ValueTask<UserResponse?> Get(string id);
}

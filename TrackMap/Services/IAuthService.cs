using TrackMap.Common.Requests;
using TrackMap.Common.Responses;

namespace TrackMap.Services;

public interface IAuthService
{
    public ValueTask<RegisterRepsonse> Register(RegisterRequest request);

    public ValueTask<LoginResponse?> Login(LoginRequest request);

    public Task Logout();
}

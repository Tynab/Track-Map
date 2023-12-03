using System.Net.Http.Json;
using TrackMap.Common.Dtos.User;
using TrackMap.Common.Requests.User;
using TrackMap.Common.Responses;
using YANLib;

namespace TrackMap.Services.Implements;

public sealed class UserService(ILogger<UserService> logger, HttpClient httpClient) : IUserService
{
    private readonly ILogger<UserService> _logger = logger;
    private readonly HttpClient _httpClient = httpClient;

    public async ValueTask<List<UserResponse>?> GetAll()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<List<UserResponse>>("api/users");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetAllUserService-Exception");

            return default;
        }
    }

    public async ValueTask<UserResponse?> Get(Guid id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<UserResponse>($"api/users/{id}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetUserService-Exception: {Id}", id);

            return default;
        }
    }

    public async ValueTask<List<UserResponse>?> Search(UserSearchDto dto)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<List<UserResponse>>(
                $"api/users/search" +
                $"?{nameof(dto.FullName).ToLowerInvariant()}={dto.FullName}" +
                $"&{nameof(dto.Email).ToLowerInvariant()}={dto.Email}" +
                $"&{nameof(dto.PhoneNumber).ToLowerInvariant()}={dto.PhoneNumber}"
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SearchUserService-Exception: {DTO}", dto.Serialize());

            return default;
        }
    }

    public async ValueTask<bool> Create(UserCreateRequest request)
    {
        try
        {
            var res = await _httpClient.PostAsJsonAsync("api/users", request);

            return res.IsSuccessStatusCode && (await res.Content.ReadAsStringAsync()).Deserialize<UserResponse>() is not null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "CreateUserService-Exception: {Request}", request.Serialize());

            return default;
        }
    }

    public async ValueTask<bool> Edit(Guid id, UserEditRequest request)
    {
        try
        {
            var res = await _httpClient.PutAsJsonAsync($"api/users/{id}", request);

            return res.IsSuccessStatusCode && (await res.Content.ReadAsStringAsync()).Deserialize<UserResponse>() is not null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "EditUserService-Exception: {Id} - {Request}", id, request.Serialize());

            return default;
        }
    }
}

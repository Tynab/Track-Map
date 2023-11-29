using System.Net.Http.Json;
using TrackMap.Common.Responses;

namespace TrackMap.Services.Implements;

public sealed class UserService(HttpClient httpClient) : IUserService
{
    private readonly HttpClient _httpClient = httpClient;

    public async ValueTask<List<UserResponse>?> GetAll() => await _httpClient.GetFromJsonAsync<List<UserResponse>>("api/users");

    public async ValueTask<UserResponse?> Get(string id) => await _httpClient.GetFromJsonAsync<UserResponse>($"api/users/{id}");
}

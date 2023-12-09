using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using TrackMap.Common.Requests;
using TrackMap.Common.Responses;
using YANLib;

namespace TrackMap.Services.Implements;

public sealed class AuthService(ILogger<AuthService> logger, HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider, ILocalStorageService localStorageService) : IAuthService
{
    private readonly ILogger<AuthService> _logger = logger;
    private readonly HttpClient _httpClient = httpClient;
    private readonly AuthenticationStateProvider _authenticationStateProvider = authenticationStateProvider;
    private readonly ILocalStorageService _localStorageService = localStorageService;

    public async ValueTask<RegisterRepsonse?> Register(RegisterRequest? request)
    {
        try
        {
            if (request is null)
            {
                return default;
            }

            var res = await _httpClient.PostAsJsonAsync("api/register", request);

            return (await res.Content.ReadAsStringAsync()).Deserialize<RegisterRepsonse>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "RegisterAuthService-Exception: {Request}", request.Serialize());

            return default;
        }
    }

    public async ValueTask<LoginResponse?> Login(LoginRequest? request)
    {
        try
        {
            if (request is null)
            {
                return default;
            }

            var res = await _httpClient.PostAsJsonAsync("api/login", request);
            var rslt = (await res.Content.ReadAsStringAsync()).Deserialize<LoginResponse>();

            if (res.IsSuccessStatusCode && rslt is not null)
            {
                await _localStorageService.SetItemAsync("authToken", rslt.Token);
                ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(request.UserName);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", rslt.Token);
            }

            return rslt;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "LoginAuthService-Exception: {Request}", request.Serialize());

            return default;
        }
    }

    public async ValueTask Logout()
    {
        try
        {
            await _localStorageService.RemoveItemAsync("authToken");
            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
            _httpClient.DefaultRequestHeaders.Authorization = default;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "LogoutAuthService-Exception");

            return;
        }
    }
}

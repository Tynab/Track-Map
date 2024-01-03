using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using YANLib;
using static System.Convert;
using static System.Security.Claims.ClaimTypes;
using static System.Text.Json.JsonSerializer;
using static System.Threading.Tasks.Task;

namespace TrackMap;

public sealed class ApiAuthenticationStateProvider(HttpClient httpClient, ILocalStorageService localStorageService) : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly ILocalStorageService _localStorageService = localStorageService;

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var tk = await _localStorageService.GetItemAsync<string>("authToken");

        if (tk.IsWhiteSpaceOrNull())
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", tk);

        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(tk), "jwt")));
    }

    public void MarkUserAsAuthenticated(string? token)
    {
        if (token.IsWhiteSpaceOrNull())
        {
            return;
        }

        NotifyAuthenticationStateChanged(FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt")))));
    }

    public void MarkUserAsLoggedOut() => NotifyAuthenticationStateChanged(FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()))));

    private static List<Claim>? ParseClaimsFromJwt(string? jwt)
    {
        if (jwt.IsWhiteSpaceOrNull())
        {
            return default;
        }

        var rslt = new List<Claim>();

        var keyValPrs = ParseBase64WithoutPadding(jwt.Split('.')[1])?.Deserialize<Dictionary<string, object>>();

        if (keyValPrs is not null && keyValPrs.TryGetValue(Role, out var rawRoles))
        {
            if (rawRoles is not null)
            {
                var sRoles = rawRoles.ToString() ?? string.Empty;

                if (sRoles.Trim().StartsWith('['))
                {
                    var roles = sRoles.Deserialize<string[]>();

                    if (roles.IsNotEmptyAndNull())
                    {
                        foreach (var role in roles)
                        {
                            rslt.Add(new Claim(Role, role));
                        }
                    }
                }
                else
                {
                    rslt.Add(new Claim(Role, sRoles));
                }

                _ = keyValPrs.Remove(Role);
            }

            rslt.AddRange(keyValPrs.Select(x => new Claim(x.Key, x.Value.ToString() ?? string.Empty)));
        }

        return rslt;
    }

    private static byte[]? ParseBase64WithoutPadding(string? base64) => base64.IsWhiteSpaceOrNull()
        ? default
        : FromBase64String($"{base64}{(base64.Length % 4) switch
        {
            2 => "==",
            3 => "=",
            _ => string.Empty
        }}");
}

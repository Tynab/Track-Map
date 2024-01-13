using Blazored.LocalStorage;
using Blazored.Toast;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TrackMap;
using TrackMap.Services;
using TrackMap.Services.Implements;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
_ = builder.Services.AddAuthorizationCore();
_ = builder.Services.AddBlazoredLocalStorage();
_ = builder.Services.AddBlazorBootstrap();
_ = builder.Services.AddBlazoredToast();
_ = builder.Services.AddTransient<IAuthService, AuthService>();
_ = builder.Services.AddTransient<IUserService, UserService>();
_ = builder.Services.AddTransient<IDeviceService, DeviceService>();
_ = builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();

_ = builder.Services.AddScoped(s => new HttpClient
{
    BaseAddress = new Uri(builder.Configuration["BackendApiUrl"] ?? "https://localhost:7083/")
});

await builder.Build().RunAsync();

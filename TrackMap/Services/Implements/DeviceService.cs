using System.Net.Http.Json;
using TrackMap.Common.Responses;

namespace TrackMap.Services.Implements;

public sealed class DeviceService : IDeviceService
{
    private readonly HttpClient _httpClient;

    public DeviceService(HttpClient httpClient) => _httpClient = httpClient;

    public async ValueTask<List<DeviceResponse>?> GetAll() => await _httpClient.GetFromJsonAsync<List<DeviceResponse>>("api/devices");

    public async ValueTask<DeviceResponse?> Get(Guid id) => await _httpClient.GetFromJsonAsync<DeviceResponse>($"api/devices/{id}");
}

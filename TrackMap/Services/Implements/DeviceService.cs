using System.Net.Http.Json;
using TrackMap.Common.Dtos.Device;
using TrackMap.Common.Requests.Device;
using TrackMap.Common.Responses;
using YANLib;

namespace TrackMap.Services.Implements;

public sealed class DeviceService(ILogger<DeviceService> logger, HttpClient httpClient) : IDeviceService
{
    private readonly ILogger<DeviceService> _logger = logger;
    private readonly HttpClient _httpClient = httpClient;

    public async ValueTask<List<DeviceResponse>?> GetAll()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<List<DeviceResponse>>("api/devices");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetAllDeviceService-Exception");

            return default;
        }
    }

    public async ValueTask<DeviceResponse?> Get(Guid id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<DeviceResponse>($"api/devices/{id}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetDeviceService-Exception: {Id}", id);

            return default;
        }
    }

    public async ValueTask<List<DeviceResponse>?> Search(DeviceSearchDto dto)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<List<DeviceResponse>>(
                $"api/devices/search" +
                $"?{nameof(dto.DeviceType).ToLowerInvariant()}={dto.DeviceType}" +
                $"&{nameof(dto.DeviceOs).ToLowerInvariant()}={dto.DeviceOs}" +
                $"&{nameof(dto.UserId).ToLowerInvariant()}={dto.UserId}"
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SearchDeviceService-Exception: {DTO}", dto.Serialize());

            return default;
        }
    }

    public async ValueTask<bool> Create(DeviceCreateRequest request)
    {
        try
        {
            var res = await _httpClient.PostAsJsonAsync("api/devices", request);

            return res.IsSuccessStatusCode && (await res.Content.ReadAsStringAsync()).Deserialize<DeviceResponse>() is not null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "CreateDeviceService-Exception: {Request}", request.Serialize());

            return default;
        }
    }

    public async ValueTask<bool> Edit(Guid id, DeviceEditRequest request)
    {
        try
        {
            var res = await _httpClient.PutAsJsonAsync($"api/devices/{id}", request);

            return res.IsSuccessStatusCode && (await res.Content.ReadAsStringAsync()).Deserialize<DeviceResponse>() is not null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "EditDeviceService-Exception: {Id} - {Request}", id, request.Serialize());

            return default;
        }
    }
}

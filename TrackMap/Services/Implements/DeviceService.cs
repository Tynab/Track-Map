using System.Net.Http.Json;
using TrackMap.Common.Dtos.Device;
using TrackMap.Common.Requests.Device;
using TrackMap.Common.Responses;
using YANLib;
using static System.Guid;
using static System.Text.Encoding;

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
            return id == Empty ? default : await _httpClient.GetFromJsonAsync<DeviceResponse>($"api/devices/{id}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetDeviceService-Exception: {Id}", id);

            return default;
        }
    }

    public async ValueTask<List<DeviceResponse>?> Search(DeviceSearchDto? dto)
    {
        try
        {
            return dto is null
                ? default
                : await _httpClient.GetFromJsonAsync<List<DeviceResponse>>(
                    $"api/devices/search" +
                    $"?{nameof(dto.DeviceType).ToLowerInvariant()}={dto.DeviceType}" +
                    $"&{nameof(dto.DeviceOs).ToLowerInvariant()}={dto.DeviceOs}" +
                    $"&{nameof(dto.IpAddress).ToLowerInvariant()}={dto.IpAddress}" +
                    $"&{nameof(dto.Latitude).ToLowerInvariant()}={dto.Latitude}" +
                    $"&{nameof(dto.Longitude).ToLowerInvariant()}={dto.Longitude}" +
                    $"&{nameof(dto.UserId).ToLowerInvariant()}={dto.UserId}" +
                    $"&{nameof(dto.CreatedBy).ToLowerInvariant()}={dto.CreatedBy}" +
                    $"&{nameof(dto.UpdatedBy).ToLowerInvariant()}={dto.UpdatedBy}" +
                    $"&{nameof(dto.Status).ToLowerInvariant()}={dto.Status}"
                );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SearchDeviceService-Exception: {DTO}", dto.Serialize());

            return default;
        }
    }

    public async ValueTask<bool> Create(DeviceCreateRequest? request)
    {
        try
        {
            if (request is null)
            {
                return default;
            }

            var res = await _httpClient.PostAsJsonAsync("api/devices", request);

            return res.IsSuccessStatusCode && (await res.Content.ReadAsStringAsync()).Deserialize<DeviceResponse>() is not null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "CreateDeviceService-Exception: {Request}", request.Serialize());

            return default;
        }
    }

    public async ValueTask<bool> Edit(Guid id, DeviceEditRequest? request)
    {
        try
        {
            if (id == Empty || request is null)
            {
                return default;
            }

            var res = await _httpClient.PutAsJsonAsync($"api/devices/{id}", request);

            return res.IsSuccessStatusCode && (await res.Content.ReadAsStringAsync()).Deserialize<DeviceResponse>() is not null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "EditDeviceService-Exception: {Id} - {Request}", id, request.Serialize());

            return default;
        }
    }

    public async ValueTask<bool> Update(Guid id, DeviceUpdateRequest? request)
    {
        try
        {
            if (id == Empty || request is null)
            {
                return default;
            }

            var res = await _httpClient.PatchAsync($"api/devices/{id}", new StringContent(request.Serialize(), UTF8, "application/json-patch+json"));

            return res.IsSuccessStatusCode && (await res.Content.ReadAsStringAsync()).Deserialize<DeviceResponse>() is not null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "UpdateDeviceService-Exception: {Id} - {Request}", id, request.Serialize());

            return default;
        }
    }

    public async ValueTask<bool> Delete(Guid id)
    {
        try
        {
            if (id == Empty)
            {
                return default;
            }

            var res = await _httpClient.DeleteAsync($"api/devices/{id}");

            return res.IsSuccessStatusCode && (await res.Content.ReadAsStringAsync()).Deserialize<DeviceResponse>() is not null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "DeleteDeviceService-Exception: {Id}", id);

            return default;
        }
    }

    public async ValueTask<bool> DeactivebyUser(Guid userId)
    {
        try
        {
            return userId != Empty && await _httpClient.GetFromJsonAsync<bool>($"api/devices/deactive-by-user?{nameof(DeviceEditRequest.UserId).ToLowerInvariant()}={userId}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "DeactivebyUserDeviceService-Exception: {UserId}", userId);

            return default;
        }
    }
}

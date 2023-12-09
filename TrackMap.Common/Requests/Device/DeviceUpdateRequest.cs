using TrackMap.Common.Enums;

namespace TrackMap.Common.Requests.Device;

public sealed class DeviceUpdateRequest
{
    public DeviceType? DeviceType { get; set; }

    public DeviceOs? DeviceOs { get; set; }

    public string? IpAddress { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public Guid? UserId { get; set; }

    public required Guid UpdatedBy { get; set; }

    public Status? Status { get; set; }
}

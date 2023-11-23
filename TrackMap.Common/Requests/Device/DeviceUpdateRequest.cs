using System.ComponentModel;

namespace TrackMap.Common.Requests.Device;

public sealed class DeviceUpdateRequest
{
    public string? DeviceType { get; set; }

    public string? DeviceOs { get; set; }

    public string? IpAddress { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longtitude { get; set; }

    public Guid? UserId { get; set; }

    public required Guid UpdatedBy { get; set; }

    [DefaultValue(true)]
    public bool? IsActive { get; set; }
}

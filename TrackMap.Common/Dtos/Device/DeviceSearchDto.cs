using TrackMap.Common.Enums;

namespace TrackMap.Common.Dtos.Device;

public sealed class DeviceSearchDto
{
    public DeviceType? DeviceType { get; set; }

    public DeviceOs? DeviceOs { get; set; }

    public string? IpAddress { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public Guid? UserId { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public Status? Status { get; set; }
}

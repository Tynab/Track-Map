using TrackMap.Common.Enums;

namespace TrackMap.Common.Requests.Device;

public sealed class DeviceEditRequest
{
    public required DeviceType DeviceType { get; set; }

    public required DeviceOs DeviceOs { get; set; }

    public required string IpAddress { get; set; }

    public required decimal Latitude { get; set; }

    public required decimal Longitude { get; set; }

    public required Guid UserId { get; set; }

    public required Guid UpdatedBy { get; set; }

    public required Status Status { get; set; }
}

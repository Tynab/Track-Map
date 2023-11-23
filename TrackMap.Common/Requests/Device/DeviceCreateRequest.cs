namespace TrackMap.Common.Requests.Device;

public sealed class DeviceCreateRequest
{
    public required string DeviceType { get; set; }

    public required string DeviceOs { get; set; }

    public required string IpAddress { get; set; }

    public required decimal Latitude { get; set; }

    public required decimal Longtitude { get; set; }

    public required Guid UserId { get; set; }

    public required Guid CreatedBy { get; set; }
}

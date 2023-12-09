using TrackMap.Common.Dtos.User;
using TrackMap.Common.Enums;

namespace TrackMap.Common.Responses;

public sealed record DeviceResponse
{
    public Guid Id { get; set; }

    public DeviceType DeviceType { get; set; }

    public DeviceOs DeviceOs { get; set; }

    public string? IpAddress { get; set; }

    public decimal Latitude { get; set; }

    public decimal Longitude { get; set; }

    public DateTime LastLogin { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Status Status { get; set; }

    public UserDto? User { get; set; }
}

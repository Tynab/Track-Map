using TrackMap.Common.Dtos.Device;

namespace TrackMap.Common.Responses;

public sealed record UserResponse
{
    public Guid Id { get; set; }

    public string? FullName { get; set; }

    public string? UserName { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public List<DeviceDto>? Devices { get; set; }
}

using TrackMap.Common.Enums;

namespace TrackMap.Common.Dtos.Device;

public sealed class DeviceSearchDto
{
    public DeviceType? DeviceType { get; set; }

    public DeviceOs? DeviceOs { get; set; }

    public Guid? UserId { get; set; }
}

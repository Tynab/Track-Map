using TrackMap.Common.Enums;
using static TrackMap.Common.Enums.DeviceOs;
using static TrackMap.Common.Enums.DeviceType;

namespace TrackMap.Common.Utilities;

public static class DeviceUtil
{
    public static DeviceType CheckType(this DeviceOs? deviceOs) => deviceOs switch
    {
        Windows or macOS or Linux or Ubuntu or ChromeOS => PC,
        iOS or Android => Smartphone,
        DeviceOs.Other or _ => DeviceType.Other
    };
}

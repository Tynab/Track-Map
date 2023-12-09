using TrackMap.Common.Enums;
using YANLib;
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

    public static DeviceOs CheckOs(this string? userAgent) => userAgent.IsWhiteSpaceOrNull()
        ? DeviceOs.Other
        : userAgent.Contains("Windows")
        ? Windows
        : userAgent.Contains("Macintosh")
        ? macOS
        : userAgent.Contains("Ubuntu")
        ? Ubuntu
        : userAgent.Contains("Linux")
        ? Linux
        : userAgent.Contains("CrOS")
        ? ChromeOS
        : userAgent.Contains("iPhone")
        ? iOS
        : userAgent.Contains("Android")
        ? Android
        : DeviceOs.Other;
}

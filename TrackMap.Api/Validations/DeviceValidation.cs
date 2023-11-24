using FluentValidation;
using TrackMap.Common.Requests.Device;

namespace TrackMap.Api.Validations;

public sealed class DeviceCreateValidator : AbstractValidator<DeviceCreateRequest>
{
    public DeviceCreateValidator()
    {
        _ = RuleFor(x => x.Latitude).GreaterThanOrEqualTo(-90).LessThanOrEqualTo(90).WithMessage("The Latitude value should be between -90 and 90 degrees");
        _ = RuleFor(x => x.Longtitude).GreaterThanOrEqualTo(-180).LessThanOrEqualTo(180).WithMessage("The Latitude value should be between -180 and 180 degrees");
    }
}

public sealed class DeviceUpdateValidator : AbstractValidator<DeviceUpdateRequest>
{
    public DeviceUpdateValidator()
    {
        _ = RuleFor(x => x.Latitude).GreaterThanOrEqualTo(-90).LessThanOrEqualTo(90).WithMessage("The Latitude value should be between -90 and 90 degrees");
        _ = RuleFor(x => x.Longtitude).GreaterThanOrEqualTo(-180).LessThanOrEqualTo(180).WithMessage("The Latitude value should be between -180 and 180 degrees");
    }
}

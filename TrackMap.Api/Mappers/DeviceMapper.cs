using AutoMapper;
using TrackMap.Api.Entities;
using TrackMap.Common.Dtos.Device;
using TrackMap.Common.Enums;
using TrackMap.Common.Requests.Device;
using TrackMap.Common.Responses;
using TrackMap.Common.SeedWork;
using YANLib;
using static System.DateTime;
using static System.Guid;

namespace TrackMap.Api.Mappers;

public sealed class DeviceMapper : Profile
{
    public DeviceMapper()
    {
        _ = CreateMap<DeviceCreateRequest, Device>()
            .ForMember(d => d.Id, o => o.MapFrom(s => NewGuid()))
            .ForMember(d => d.LastLogin, o => o.MapFrom(s => Now))
            .ForMember(d => d.CreatedAt, o => o.MapFrom(s => Now))
            .ForMember(d => d.DeviceType, o => o.MapFrom(s => s.DeviceType.ToString()))
            .ForMember(d => d.DeviceOs, o => o.MapFrom(s => s.DeviceOs.ToString()))
            .ForMember(d => d.IsActive, o => o.MapFrom(s => true))
            .ForMember(d => d.UpdatedBy, o => o.Ignore())
            .ForMember(d => d.UpdatedAt, o => o.Ignore());

        _ = CreateMap<Device, DeviceResponse>()
            .ForMember(d => d.DeviceType, o => o.MapFrom(s => s.DeviceType.IsWhiteSpaceOrNull() ? default : s.DeviceType.ToEnum<DeviceType>()))
            .ForMember(d => d.DeviceOs, o => o.MapFrom(s => s.DeviceOs.IsWhiteSpaceOrNull() ? default : s.DeviceOs.ToEnum<DeviceOs>()))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.IsActive == true ? Status.Active : Status.Inactive));

        _ = CreateMap<Device, DeviceDto>()
            .ForMember(d => d.DeviceType, o => o.MapFrom(s => s.DeviceType.IsWhiteSpaceOrNull() ? default : s.DeviceType.ToEnum<DeviceType>()))
            .ForMember(d => d.DeviceOs, o => o.MapFrom(s => s.DeviceOs.IsWhiteSpaceOrNull() ? default : s.DeviceOs.ToEnum<DeviceOs>()))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.IsActive == true ? Status.Active : Status.Inactive));

        _ = CreateMap<PagedList<Device>, PagedList<DeviceResponse>>();
    }
}

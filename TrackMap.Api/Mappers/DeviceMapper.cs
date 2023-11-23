using AutoMapper;
using TrackMap.Api.Entities;
using TrackMap.Common.Dtos;
using TrackMap.Common.Requests.Device;
using TrackMap.Common.Responses;
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
            .ForMember(d => d.IsActive, o => o.MapFrom(s => true))
            .ForMember(d => d.UpdatedBy, o => o.Ignore())
            .ForMember(d => d.UpdatedAt, o => o.Ignore());

        _ = CreateMap<Device, DeviceResponse>();

        _ = CreateMap<Device, DeviceDto>();
    }
}

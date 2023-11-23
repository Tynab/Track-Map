using AutoMapper;
using TrackMap.Api.Entities;
using TrackMap.Common.Dtos;
using TrackMap.Common.Requests.User;
using TrackMap.Common.Responses;
using static System.DateTime;
using static System.Guid;

namespace TrackMap.Api.Mappers;

public sealed class UserMapper : Profile
{
    public UserMapper()
    {
        _ = CreateMap<UserCreateRequest, User>()
            .ForMember(d => d.Id, o => o.MapFrom(s => NewGuid()))
            .ForMember(d => d.CreatedAt, o => o.MapFrom(s => Now))
            .ForMember(d => d.IsActive, o => o.MapFrom(s => true))
            .ForMember(d => d.NormalizedUserName, o => o.MapFrom(s => s.UserName.ToUpperInvariant()))
            .ForMember(d => d.NormalizedEmail, o => o.MapFrom(s => s.Email.ToUpperInvariant()))
            .ForMember(d => d.SecurityStamp, o => o.MapFrom(s => NewGuid()))
            .ForMember(d => d.UpdatedBy, o => o.Ignore())
            .ForMember(d => d.UpdatedAt, o => o.Ignore())
            .ForMember(d => d.EmailConfirmed, o => o.Ignore())
            .ForMember(d => d.PasswordHash, o => o.Ignore())
            .ForMember(d => d.ConcurrencyStamp, o => o.Ignore())
            .ForMember(d => d.PhoneNumberConfirmed, o => o.Ignore())
            .ForMember(d => d.TwoFactorEnabled, o => o.Ignore())
            .ForMember(d => d.LockoutEnd, o => o.Ignore())
            .ForMember(d => d.LockoutEnabled, o => o.Ignore())
            .ForMember(d => d.AccessFailedCount, o => o.Ignore());

        _ = CreateMap<User, UserResponse>();

        _ = CreateMap<User, UserDto>();
    }
}

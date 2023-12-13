using Microsoft.AspNetCore.Identity;
using TrackMap.Api.Entities;
using TrackMap.Common.Enums;
using TrackMap.Common.Utilities;
using YANLib;
using static System.DateTime;
using static System.Enum;
using static System.Guid;
using static System.Linq.Enumerable;
using static YANLib.YANNum;

namespace TrackMap.Api.Data;

public sealed class TrackMapDbContextSeed
{
    private readonly PasswordHasher<User> _passwordHasher = new();

    public async Task SeedAsync(ILogger<TrackMapDbContextSeed>? logger, TrackMapDbContext context)
    {
        try
        {
            var userIds = Range(0, 8).Select(i => NewGuid()).ToArray();

            if (!context.Users.Any())
            {
                var lastNames = new string[] { "Nguyễn", "Trần", "Lê", "Phạm", "Phan", "Trương", "Bùi", "Đặng" };
                var midNames = new string[] { "Văn", "Thị", "Văn", "Thị", "Văn", "Thị", "Văn", "Thị" };
                var firstNames = new string[] { "A", "B", "C", "D", "E", "G", "H", "I" };
                var userNames = new string[] { "nva", "ttb", "lvc", "ptd", "pve", "ttg", "bvh", "dti" };

                await context.Users.AddRangeAsync(Range(0, 8).Select(i =>
                {
                    var email = $"{userNames[i]}@gmail.com";

                    var user = new User
                    {
                        Id = userIds[i],
                        FullName = $"{lastNames[i]} {midNames[i]} {firstNames[i]}",
                        Email = email,
                        NormalizedEmail = email.ToUpperInvariant(),
                        PhoneNumber = $"0{GenerateRandomInt(111111111, 999999999)}",
                        CreatedBy = userIds[i],
                        CreatedAt = Now,
                        SecurityStamp = NewGuid().ToString(),
                        UserName = userNames[i],
                        NormalizedUserName = userNames[i].ToUpperInvariant()
                    };

                    user.PasswordHash = _passwordHasher.HashPassword(user, $"{userNames[i].ToCapitalize()}@123");

                    return user;

                }));
            }

            var roleIds = Range(0, 8).Select(i => NewGuid()).ToArray();

            if (!context.Roles.Any())
            {
                await context.Roles.AddRangeAsync(Range(0, 8).Select(i => new Role
                {
                    Id = roleIds[i],
                    Name = i is 0 ? "Admin" : "User",
                    CreatedBy = userIds[i],
                    CreatedAt = Now
                }));
            }

            if (!context.UserRoles.Any())
            {
                await context.UserRoles.AddRangeAsync(Range(0, 8).Select(i => new IdentityUserRole<Guid>
                {
                    UserId = userIds[i],
                    RoleId = roleIds[i]
                }));
            }

            if (!context.Devices.Any())
            {
                var lats = new decimal[] { 10.803140162639943m, 10.79034727955936m, 10.772253852070987m, 10.836946301832068m, 10.857962793166275m, 10.822174246205119m, 10.773312964050522m, 10.764001751194433m };
                var lngs = new decimal[] { 106.63803618847192m, 106.70441598348812m, 106.67987342754463m, 106.66718421214821m, 106.75619861929913m, 106.6868427936832m, 106.66864600773071m, 106.69611663977756m };

                await context.Devices.AddRangeAsync(Range(0, 8).Select(i =>
                {
                    var oss = GetNames<DeviceOs>();
                    var ips = GenerateRandomBytes(size: 4);

                    return new Device
                    {
                        Id = NewGuid(),
                        DeviceType = oss[i].ToEnum<DeviceOs>().CheckType().ToString(),
                        DeviceOs = oss[i],
                        IpAddress = string.Join(".", ips.Select(ip => ip.ToString())),
                        Latitude = lats[i],
                        Longitude = lngs[i],
                        LastLogin = Now,
                        UserId = userIds[i],
                        CreatedBy = userIds[i],
                        CreatedAt = Now,
                        IsActive = true
                    };
                }));
            }

            _ = await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            logger?.LogError(ex, "SeedAsyncTrackMapDbContextSeed-Exception");

            throw;
        }
    }
}

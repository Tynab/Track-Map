using Microsoft.AspNetCore.Identity;
using TrackMap.Api.Entities;
using static System.DateTime;
using static System.Guid;

namespace TrackMap.Api.Data;

public sealed class TrackMapDbContextSeed
{
    private readonly IPasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

    public async Task SeedAsync(ILogger<TrackMapDbContextSeed> logger, TrackMapDbContext context)
    {
        try
        {
            var userId = NewGuid();

            if (!context.Users.Any())
            {
                var entity = new User
                {
                    Id = userId,
                    FirstName = "An",
                    LastName = "Yami",
                    Email = "yamian@gmail.com",
                    NormalizedEmail = "YAMIAN@GMAIL.COM",
                    PhoneNumber = "0123456789",
                    CreatedBy = userId,
                    CreatedAt = Now,
                    IsActive = true,
                    SecurityStamp = NewGuid().ToString(),
                    UserName = "yan",
                    NormalizedUserName = "YAN"
                };

                entity.PasswordHash = _passwordHasher.HashPassword(entity, "Admin@123");
                _ = await context.Users.AddAsync(entity);
            }

            if (!context.Devices.Any())
            {
                _ = await context.Devices.AddAsync(new Device
                {
                    Id = NewGuid(),
                    DeviceType = "PC",
                    DeviceOs = "Windows",
                    IpAddress = "115.78.238.133",
                    Latitude = 10.803154696406853m,
                    Longtitude = 106.63791228756241m,
                    LastLogin = Now,
                    UserId = userId,
                    CreatedBy = userId,
                    CreatedAt = Now,
                    IsActive = true,
                });
            }

            _ = await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "SeedAsyncTrackMapDbContextSeed-Exception");

            throw;
        }
    }
}

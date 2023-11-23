using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TrackMap.Api.Entities;

namespace TrackMap.Api.Data;

public sealed class TrackMapDbContext : IdentityDbContext<User, Role, Guid>
{
    public TrackMapDbContext(DbContextOptions<TrackMapDbContext> options) : base(options)
    {
    }

    public DbSet<Device> Devices { get; set; }
}

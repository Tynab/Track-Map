using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TrackMap.Api.Entities;

namespace TrackMap.Api.Data;

public sealed class TrackMapDbContext(DbContextOptions<TrackMapDbContext> options) : IdentityDbContext<User, Role, Guid>(options)
{
    public DbSet<Device> Devices { get; set; }
}

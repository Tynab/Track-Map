using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TrackMap.Api.Data;
using TrackMap.Api.Extensions;
using TrackMap.Api.Options;
using TrackMap.Api.Repositories;
using TrackMap.Api.Repositories.Implements;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureOptions<DatabaseOptionsSetup>();
builder.Services.AddDbContext<TrackMapDbContext>((p, o) => o.UseSqlServer(p.GetService<IOptions<DatabaseOptions>>()?.Value.ConnectionString));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IDeviceRepository, DeviceRepository>();
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    _ = app.UseSwagger();
    _ = app.UseSwaggerUI();
}

app.MigrateDbContext<TrackMapDbContext>((c, s) => new TrackMapDbContextSeed().SeedAsync(s.GetService<ILogger<TrackMapDbContextSeed>>()!, c).Wait());
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

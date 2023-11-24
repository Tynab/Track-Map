using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TrackMap.Api.Data;
using TrackMap.Api.Entities;
using TrackMap.Api.Extensions;
using TrackMap.Api.Options;
using TrackMap.Api.Repositories;
using TrackMap.Api.Repositories.Implements;
using TrackMap.Api.Validations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureOptions<DatabaseOptionsSetup>();
builder.Services.AddDbContext<TrackMapDbContext>((p, o) => o.UseSqlServer(p.GetService<IOptions<DatabaseOptions>>()?.Value.ConnectionString));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o => o.EnableAnnotations());
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IDeviceRepository, DeviceRepository>();
builder.Services.AddCors(o => o.AddPolicy("CorsPolicy", b => b.SetIsOriginAllowed((h) => true).AllowAnyMethod().AllowAnyHeader().AllowCredentials()));
builder.Services.AddIdentity<User, Role>().AddEntityFrameworkStores<TrackMapDbContext>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<DeviceCreateValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<DeviceUpdateValidator>();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o => o.TokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    ValidIssuer = builder.Configuration["JwtIssuer"],
    ValidAudience = builder.Configuration["JwtAudience"],
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSecurityKey"] ?? string.Empty))
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    _ = app.UseSwagger();
    _ = app.UseSwaggerUI();
}

app.MigrateDbContext<TrackMapDbContext>((c, s) => new TrackMapDbContextSeed().SeedAsync(s.GetService<ILogger<TrackMapDbContextSeed>>()!, c).Wait());
app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

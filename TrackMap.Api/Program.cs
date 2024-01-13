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

_ = builder.Services.ConfigureOptions<DatabaseOptionsSetup>();
_ = builder.Services.AddDbContext<TrackMapDbContext>((p, o) => o.UseSqlServer(p.GetService<IOptions<DatabaseOptions>>()?.Value.ConnectionString));
_ = builder.Services.AddControllers();
_ = builder.Services.AddEndpointsApiExplorer();
_ = builder.Services.AddSwaggerGen(o => o.EnableAnnotations());
_ = builder.Services.AddTransient<IUserRepository, UserRepository>();
_ = builder.Services.AddTransient<IDeviceRepository, DeviceRepository>();
_ = builder.Services.AddCors(o => o.AddPolicy("CorsPolicy", b => b.SetIsOriginAllowed((h) => true).AllowAnyMethod().AllowAnyHeader().AllowCredentials()));
_ = builder.Services.AddIdentity<User, Role>().AddEntityFrameworkStores<TrackMapDbContext>();
_ = builder.Services.AddFluentValidationAutoValidation();
_ = builder.Services.AddFluentValidationClientsideAdapters();
_ = builder.Services.AddValidatorsFromAssemblyContaining<DeviceCreateValidator>();
_ = builder.Services.AddValidatorsFromAssemblyContaining<DeviceUpdateValidator>();
_ = builder.Services.AddAutoMapper(typeof(Program));

_ = builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o => o.TokenValidationParameters = new TokenValidationParameters
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

_ = app.MigrateDbContext<TrackMapDbContext>((c, s) => new TrackMapDbContextSeed().SeedAsync(s.GetService<ILogger<TrackMapDbContextSeed>>(), c).Wait());
_ = app.UseHttpsRedirection();
_ = app.UseCors("CorsPolicy");
_ = app.UseAuthentication();
_ = app.UseAuthorization();
_ = app.MapControllers();
app.Run();

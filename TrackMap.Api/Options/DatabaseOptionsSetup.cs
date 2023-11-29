using Microsoft.Extensions.Options;

namespace TrackMap.Api.Options;

public sealed class DatabaseOptionsSetup(IConfiguration configuration) : IConfigureOptions<DatabaseOptions>
{
    private readonly IConfiguration _configuration = configuration;

    public void Configure(DatabaseOptions options) => options.ConnectionString = _configuration.GetConnectionString("Default")!;
}

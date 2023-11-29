using Microsoft.EntityFrameworkCore;
using TrackMap.Api.Data;
using TrackMap.Api.Entities;
using YANLib;

namespace TrackMap.Api.Repositories.Implements;

public sealed class DeviceRepository(ILogger<DeviceRepository> logger, TrackMapDbContext dbContext) : IDeviceRepository
{
    private readonly ILogger<DeviceRepository> _logger = logger;
    private readonly TrackMapDbContext _dbContext = dbContext;

    public async ValueTask<IEnumerable<Device>> GetAll()
    {
        try
        {
            return await _dbContext.Devices.Where(x => x.IsActive == true).Include(x => x.User).AsNoTracking().ToArrayAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetAllDeviceRepository-Exception");

            throw;
        }
    }

    public async ValueTask<Device?> Get(Guid id)
    {
        try
        {
            return await _dbContext.Devices.Include(x => x.User).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id && x.IsActive == true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetDeviceRepository-Exception: {Id}", id);

            throw;
        }
    }

    public async ValueTask<Device?> Create(Device entity)
    {
        try
        {
            var entry = await _dbContext.Devices.AddAsync(entity);

            if (await _dbContext.SaveChangesAsync() > 0)
            {
                entry.Entity.User = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == entity.UserId && x.IsActive == true);

                return entry.Entity;
            }
            else
            {
                return default;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "CreateDeviceRepository-Exception: {Entity}", entity.Serialize());

            throw;
        }
    }

    public async ValueTask<Device?> Update(Device entity)
    {
        try
        {
            var entry = _dbContext.Update(entity);

            if (await _dbContext.SaveChangesAsync() > 0)
            {
                entry.Entity.User = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == entry.Entity.UserId && x.IsActive == true);

                return entry.Entity;
            }
            else
            {
                return default;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "UpdateDeviceRepository-Exception: {Entity}", entity.Serialize());

            throw;
        }
    }
}

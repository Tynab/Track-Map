﻿using Microsoft.EntityFrameworkCore;
using TrackMap.Api.Data;
using TrackMap.Api.Entities;
using TrackMap.Common.Dtos.User;
using TrackMap.Common.SeedWork;
using YANLib;

namespace TrackMap.Api.Repositories.Implements;

public sealed class UserRepository(ILogger<UserRepository> logger, TrackMapDbContext dbContext) : IUserRepository
{
    private readonly ILogger<UserRepository> _logger = logger;
    private readonly TrackMapDbContext _dbContext = dbContext;

    public async ValueTask<IEnumerable<User>> GetAll()
    {
        try
        {
            return await _dbContext.Users.OrderBy(x => x.FullName).Include(x => x.Devices).AsNoTracking().ToArrayAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetAllUserRepository-Exception");

            throw;
        }
    }

    public async ValueTask<User?> Get(Guid id)
    {
        try
        {
            return await _dbContext.Users.Include(x => x.Devices).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetUserRepository-Exception: {Id}", id);

            throw;
        }
    }

    public async ValueTask<PagedList<User>> Search(UserSearchDto dto)
    {
        try
        {
            var qry = _dbContext.Users.AsQueryable();

            if (dto.FullName.IsNotWhiteSpaceAndNull())
            {
                qry = qry.Where(x => x.FullName!.Contains(dto.FullName));
            }

            if (dto.Email.IsNotWhiteSpaceAndNull())
            {
                qry = qry.Where(x => x.Email!.Contains(dto.Email));
            }

            if (dto.PhoneNumber.IsNotWhiteSpaceAndNull())
            {
                qry = qry.Where(x => x.PhoneNumber!.Contains(dto.PhoneNumber));
            }

            if (dto.UserName.IsNotWhiteSpaceAndNull())
            {
                qry = qry.Where(x => x.UserName!.Contains(dto.UserName));
            }

            if (dto.CreatedBy.HasValue)
            {
                qry = qry.Where(x => x.CreatedBy == dto.CreatedBy.Value);
            }

            if (dto.UpdatedBy.HasValue)
            {
                qry = qry.Where(x => x.UpdatedBy == dto.UpdatedBy.Value);
            }

            return new PagedList<User>(
                await qry.OrderBy(x => x.FullName).Skip((dto.PageNumber - 1) * dto.PageSize).Take(dto.PageSize).Include(x => x.Devices).AsNoTracking().ToListAsync(),
                await qry.CountAsync(),
                dto.PageNumber,
                dto.PageSize
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SearchUserRepository-Exception: {DTO}", dto.Serialize());

            throw;
        }
    }

    public async ValueTask<User?> Create(User entity)
    {
        try
        {
            var entry = await _dbContext.Users.AddAsync(entity);

            return await _dbContext.SaveChangesAsync() > 0 ? entry.Entity : default;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "CreateUserRepository-Exception: {Entity}", entity.Serialize());

            throw;
        }
    }

    public async ValueTask<User?> Update(User entity)
    {
        try
        {
            var entry = _dbContext.Update(entity);

            if (await _dbContext.SaveChangesAsync() > 0)
            {
                entry.Entity.Devices = await _dbContext.Devices.Where(x => x.UserId == entity.Id).AsNoTracking().ToArrayAsync();

                return entry.Entity;
            }
            else
            {
                return default;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "UpdateUserRepository-Exception: {Entity}", entity.Serialize());

            throw;
        }
    }

    public async ValueTask<User?> Delete(User entity)
    {
        try
        {
            var ent = _dbContext.Users.Remove(entity);

            return await _dbContext.SaveChangesAsync() > 0 ? ent.Entity : default;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "DeleteUserRepository-Exception: {Entity}", entity.Serialize());

            throw;
        }
    }
}

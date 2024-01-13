using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using TrackMap.Api.Entities;
using TrackMap.Api.Repositories;
using TrackMap.Common.Dtos.User;
using TrackMap.Common.Requests.User;
using TrackMap.Common.Responses;
using TrackMap.Common.SeedWork;
using YANLib;
using static System.DateTime;

namespace TrackMap.Api.Controllers;

[ApiController]
[Route("api/users")]
public sealed class UserController(ILogger<UserController> logger, IMapper mapper, IUserRepository repository) : ControllerBase
{
    private readonly ILogger<UserController> _logger = logger;
    private readonly IMapper _mapper = mapper;
    private readonly IUserRepository _repository = repository;
    private readonly PasswordHasher<User> _passwordHasher = new();

    [HttpGet]
    [SwaggerOperation(Summary = "Get all Users")]
    public async ValueTask<IActionResult> GetAll()
    {
        try
        {
            var rslt = _mapper.Map<IEnumerable<UserResponse>>(await _repository.GetAll()).ToList();

            return rslt.IsEmptyOrNull() ? NotFound() : Ok(rslt);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetAllUserController-Exception");

            return Problem();
        }
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get User by Id")]
    public async ValueTask<IActionResult> Get(Guid id)
    {
        try
        {
            var ent = await _repository.Get(id);

            return ent is null ? NotFound(new
            {
                title = "Not Found",
                status = 404,
                errors = new
                {
                    id,
                }
            }) : Ok(_mapper.Map<UserResponse>(ent));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetUserController-Exception: {Id}", id);

            return Problem();
        }
    }

    [HttpGet("search")]
    [SwaggerOperation(Summary = "Search Users")]
    public async ValueTask<IActionResult> Search([FromQuery] UserSearchDto dto)
    {
        try
        {
            return Ok(_mapper.Map<PagedList<UserResponse>>(await _repository.Search(dto)));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SearchUserController-Exception: {DTO}", dto.Serialize());

            throw;
        }
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Create User")]
    public async ValueTask<IActionResult> Create([Required] UserCreateRequest request)
    {
        try
        {
            if ((await _repository.Search(new UserSearchDto { UserName = request.UserName }))?.Items?.Count > 0)
            {
                return BadRequest(new
                {
                    title = $"{nameof(request.UserName)} alraedy exists.",
                    status = 400,
                    errors = new
                    {
                        request.UserName
                    }
                });
            }

            if (await _repository.Get(request.CreatedBy) is null)
            {
                return NotFound(new
                {
                    title = $"{nameof(request.CreatedBy)} not found {nameof(User)}.",
                    status = 404,
                    errors = new
                    {
                        id = request.CreatedBy
                    }
                });
            }

            var ent = _mapper.Map<User>(request);

            ent.PasswordHash = _passwordHasher.HashPassword(ent, request.Password);

            var rslt = await _repository.Create(ent);

            return rslt is null ? Problem() : Ok(_mapper.Map<UserResponse>(rslt));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "CreateUserController-Exception: {Request}", request.Serialize());

            return Problem();
        }
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Edit User")]
    public async ValueTask<IActionResult> Edit(Guid id, [Required] UserEditRequest request)
    {
        try
        {
            var ent = await _repository.Get(id);

            if (ent is null)
            {
                return NotFound(new
                {
                    title = "Not Found",
                    status = 404,
                    errors = new
                    {
                        id,
                    }
                });
            }

            if (await _repository.Get(request.UpdatedBy) is null)
            {
                return NotFound(new
                {
                    title = $"{nameof(request.UpdatedBy)} not found {nameof(User)}.",
                    status = 404,
                    errors = new
                    {
                        id = request.UpdatedBy
                    }
                });
            }

            ent.FullName = request.FullName;
            ent.Email = request.Email;
            ent.NormalizedEmail = request.Email.IsWhiteSpaceOrNull() ? string.Empty : request.Email.ToUpperInvariant();
            ent.PhoneNumber = request.PhoneNumber;
            ent.PasswordHash = _passwordHasher.HashPassword(ent, request.Password);
            ent.UpdatedBy = request.UpdatedBy;
            ent.UpdatedAt = Now;
            ent.Devices = null;

            var rslt = await _repository.Update(ent);

            return rslt is null ? Problem() : Ok(_mapper.Map<UserResponse>(rslt));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "EditUserController-Exception: {Id} - {Request}", id, request.Serialize());

            return Problem();
        }
    }

    [HttpPatch("{id}")]
    [SwaggerOperation(Summary = "Update User")]
    public async ValueTask<IActionResult> Update(Guid id, [Required] UserUpdateRequest request)
    {
        try
        {
            var ent = await _repository.Get(id);

            if (ent is null)
            {
                return NotFound(new
                {
                    title = "Not Found",
                    status = 404,
                    errors = new
                    {
                        id,
                    }
                });
            }

            if (await _repository.Get(request.UpdatedBy) is null)
            {
                return NotFound(new
                {
                    title = $"{nameof(request.UpdatedBy)} not found {nameof(User)}.",
                    status = 404,
                    errors = new
                    {
                        id = request.UpdatedBy
                    }
                });
            }

            if (request.FullName.IsNotWhiteSpaceAndNull())
            {
                ent.FullName = request.FullName;
            }

            if (request.Email.IsNotWhiteSpaceAndNull())
            {
                ent.Email = request.Email;
                ent.NormalizedEmail = request.Email.ToUpperInvariant();
            }

            if (request.PhoneNumber.IsNotWhiteSpaceAndNull())
            {
                ent.PhoneNumber = request.PhoneNumber;
            }

            if (request.Password.IsNotWhiteSpaceAndNull())
            {
                ent.PasswordHash = _passwordHasher.HashPassword(ent, request.Password);
            }

            ent.UpdatedBy = request.UpdatedBy;
            ent.UpdatedAt = Now;
            ent.Devices = null;

            var rslt = await _repository.Update(ent);

            return rslt is null ? Problem() : Ok(_mapper.Map<UserResponse>(rslt));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "UpdateUserController-Exception: {Id} - {Request}", id, request.Serialize());

            return Problem();
        }
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Delete User")]
    public async ValueTask<IActionResult> Delete(Guid id)
    {
        try
        {
            var ent = await _repository.Get(id);

            if (ent is null)
            {
                return NotFound(new
                {
                    title = "Not Found",
                    status = 404,
                    errors = new
                    {
                        id,
                    }
                });
            }

            var rslt = await _repository.Delete(ent);

            return rslt is null ? Problem() : Ok(_mapper.Map<UserResponse>(rslt));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "DeleteUserController-Exception: {Id}", id);

            return Problem();
        }
    }
}

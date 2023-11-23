using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using TrackMap.Api.Entities;
using TrackMap.Api.Repositories;
using TrackMap.Common.Requests.User;
using TrackMap.Common.Responses;
using YANLib;
using static System.DateTime;

namespace TrackMap.Api.Controllers;

[Route("api/users")]
[ApiController]
public sealed class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IMapper _mapper;
    private readonly IUserRepository _repository;
    private readonly IPasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

    public UserController(ILogger<UserController> logger, IMapper mapper, IUserRepository repository)
    {
        _logger = logger;
        _mapper = mapper;
        _repository = repository;
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Get all Users")]
    public async ValueTask<IActionResult> GetAll()
    {
        try
        {
            var rslt = _mapper.Map<IEnumerable<UserResponse>>(await _repository.GetAll());

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

    [HttpPost]
    [SwaggerOperation(Summary = "Create Users")]
    public async ValueTask<IActionResult> Create([Required] UserCreateRequest request)
    {
        try
        {
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
            _logger.LogError(ex, "CreateUserController-Exception: {Request}", request.CamelSerialize());

            return Problem();
        }
    }

    [HttpPatch("{id}")]
    [SwaggerOperation(Summary = "Update Users")]
    public async ValueTask<IActionResult> Update(Guid id, UserUpdateRequest request)
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

            if (request.FirstName!.IsNotWhiteSpaceAndNull())
            {
                ent.FirstName = request.FirstName;
            }

            if (request.LastName!.IsNotWhiteSpaceAndNull())
            {
                ent.LastName = request.LastName;
            }

            if (request.Email!.IsNotWhiteSpaceAndNull())
            {
                ent.Email = request.Email;
                ent.NormalizedEmail = request.Email!.ToUpper();
            }

            if (request.PhoneNumber!.IsNotWhiteSpaceAndNull())
            {
                ent.PhoneNumber = request.PhoneNumber;
            }

            if (request.IsActive.HasValue)
            {
                ent.IsActive = request.IsActive.Value;
            }

            if (request.Password!.IsNotWhiteSpaceAndNull())
            {
                ent.PasswordHash = _passwordHasher.HashPassword(ent, request.Password!);
            }

            ent.UpdatedBy = request.UpdatedBy;
            ent.UpdatedAt = Now;

            var rslt = await _repository.Update(ent);

            return rslt is null ? Problem() : Ok(_mapper.Map<UserResponse>(rslt));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "UpdateUserController-Exception: {Id} - {Request}", id, request.CamelSerialize());

            return Problem();
        }
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using TrackMap.Api.Entities;
using TrackMap.Api.Repositories;
using TrackMap.Common.Requests.Device;
using TrackMap.Common.Responses;
using YANLib;
using static System.DateTime;

namespace TrackMap.Api.Controllers;

[Route("api/devices")]
[ApiController]
public sealed class DeviceController : ControllerBase
{
    private readonly ILogger<DeviceController> _logger;
    private readonly IMapper _mapper;
    private readonly IDeviceRepository _repository;
    private readonly IUserRepository _userRepository;

    public DeviceController(ILogger<DeviceController> logger, IMapper mapper, IDeviceRepository repository, IUserRepository userRepository)
    {
        _logger = logger;
        _mapper = mapper;
        _repository = repository;
        _userRepository = userRepository;
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Get all Devices")]
    public async ValueTask<IActionResult> GetAll()
    {
        try
        {
            var rslt = _mapper.Map<IEnumerable<DeviceResponse>>(await _repository.GetAll());

            return rslt.IsEmptyOrNull() ? NotFound() : Ok(rslt);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetAllDeviceController-Exception");

            return Problem();
        }
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get Device by Id")]
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
            }) : Ok(_mapper.Map<DeviceResponse>(ent));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetDeviceController-Exception: {Id}", id);

            return Problem();
        }
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Create Device")]
    public async ValueTask<IActionResult> Create([Required] DeviceCreateRequest request)
    {
        try
        {
            if (await _userRepository.Get(request.CreatedBy) is null)
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

            var rslt = await _repository.Create(_mapper.Map<Device>(request));

            return rslt is null ? Problem() : Ok(_mapper.Map<DeviceResponse>(rslt));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "CreateDeviceController-Exception: {Request}", request.CamelSerialize());

            return Problem();
        }
    }

    [HttpPatch("{id}")]
    [SwaggerOperation(Summary = "Update Device")]
    public async ValueTask<IActionResult> Update(Guid id, DeviceUpdateRequest request)
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

            if (await _userRepository.Get(request.UpdatedBy) is null)
            {
                return NotFound(new
                {
                    title = $"{nameof(request.UpdatedBy)} not found {nameof(User)}.",
                    status = 404,
                    errors = new
                    {
                        request.UpdatedBy
                    }
                });
            }

            if (request.DeviceType!.IsNotWhiteSpaceAndNull())
            {
                ent.DeviceType = request.DeviceType;
            }

            if (request.DeviceOs!.IsNotWhiteSpaceAndNull())
            {
                ent.DeviceOs = request.DeviceOs;
            }

            if (request.IpAddress!.IsNotWhiteSpaceAndNull())
            {
                ent.IpAddress = request.IpAddress;
            }

            if (request.Latitude.HasValue)
            {
                ent.Latitude = request.Latitude.Value;
            }

            if (request.Longtitude.HasValue)
            {
                ent.Longtitude = request.Longtitude.Value;
            }

            if (request.UserId.HasValue)
            {
                ent.UserId = request.UserId.Value;
            }

            if (request.IsActive.HasValue)
            {
                ent.IsActive = request.IsActive.Value;
            }

            ent.LastLogin = Now;
            ent.UpdatedBy = request.UpdatedBy;
            ent.UpdatedAt = Now;

            var rslt = await _repository.Update(ent);

            return rslt is null ? Problem() : Ok(_mapper.Map<DeviceResponse>(rslt));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "UpdateDeviceController-Exception: {Id} - {Request}", id, request.CamelSerialize());

            return Problem();
        }
    }
}

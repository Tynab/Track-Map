using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using TrackMap.Api.Entities;
using TrackMap.Api.Repositories;
using TrackMap.Common.Dtos.Device;
using TrackMap.Common.Requests.Device;
using TrackMap.Common.Responses;
using TrackMap.Common.SeedWork;
using YANLib;
using static System.DateTime;

namespace TrackMap.Api.Controllers;

[Route("api/devices")]
[ApiController]
public sealed class DeviceController(ILogger<DeviceController> logger, IMapper mapper, IDeviceRepository repository, IUserRepository userRepository) : ControllerBase
{
    private readonly ILogger<DeviceController> _logger = logger;
    private readonly IMapper _mapper = mapper;
    private readonly IDeviceRepository _repository = repository;
    private readonly IUserRepository _userRepository = userRepository;

    [HttpGet]
    [SwaggerOperation(Summary = "Get all Devices")]
    public async ValueTask<IActionResult> GetAll()
    {
        try
        {
            var rslt = _mapper.Map<IEnumerable<DeviceResponse>>(await _repository.GetAll()).ToList();

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

    [HttpGet("search")]
    [SwaggerOperation(Summary = "Search Devices")]
    public async ValueTask<IActionResult> Search([FromQuery] DeviceSearchDto dto)
    {
        try
        {
            return Ok(_mapper.Map<PagedList<DeviceResponse>>(await _repository.Search(dto)));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SearchDeviceController-Exception: {DTO}", dto.Serialize());

            throw;
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
            _logger.LogError(ex, "CreateDeviceController-Exception: {Request}", request.Serialize());

            return Problem();
        }
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Edit Device")]
    public async ValueTask<IActionResult> Edit(Guid id, [Required] DeviceEditRequest request)
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

            ent.DeviceType = request.DeviceType.ToString();
            ent.DeviceOs = request.DeviceOs.ToString();
            ent.IpAddress = request.IpAddress;
            ent.Latitude = request.Latitude;
            ent.Longitude = request.Longitude;
            ent.UserId = request.UserId;
            ent.LastLogin = Now;
            ent.UpdatedBy = request.UpdatedBy;
            ent.UpdatedAt = Now;
            ent.IsActive = request.Status.ToBool();
            ent.User = null;

            var rslt = await _repository.Update(ent);

            return rslt is null ? Problem() : Ok(_mapper.Map<DeviceResponse>(rslt));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "EditDeviceController-Exception: {Id} - {Request}", id, request.Serialize());

            return Problem();
        }
    }

    [HttpPatch("{id}")]
    [SwaggerOperation(Summary = "Update Device")]
    public async ValueTask<IActionResult> Update(Guid id, [Required] DeviceUpdateRequest request)
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

            if (request.DeviceType.HasValue)
            {
                ent.DeviceType = request.DeviceType.Value.ToString();
            }

            if (request.DeviceOs.HasValue)
            {
                ent.DeviceOs = request.DeviceOs.Value.ToString();
            }

            if (request.IpAddress.IsNotWhiteSpaceAndNull())
            {
                ent.IpAddress = request.IpAddress;
            }

            if (request.Latitude.HasValue)
            {
                ent.Latitude = request.Latitude.Value;
            }

            if (request.Longitude.HasValue)
            {
                ent.Longitude = request.Longitude.Value;
            }

            if (request.UserId.HasValue)
            {
                ent.UserId = request.UserId.Value;
            }

            if (request.Status.HasValue)
            {
                ent.IsActive = request.Status.Value.ToBool();
            }

            ent.LastLogin = Now;
            ent.UpdatedBy = request.UpdatedBy;
            ent.UpdatedAt = Now;
            ent.User = null;

            var rslt = await _repository.Update(ent);

            return rslt is null ? Problem() : Ok(_mapper.Map<DeviceResponse>(rslt));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "UpdateDeviceController-Exception: {Id} - {Request}", id, request.Serialize());

            return Problem();
        }
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Delete Device")]
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

            return rslt is null ? Problem() : Ok(_mapper.Map<DeviceResponse>(rslt));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "DeleteDeviceController-Exception: {Id}", id);

            return Problem();
        }
    }

    [HttpGet("deactive-by-user")]
    [SwaggerOperation(Summary = "Deactive Devices by User")]
    public async ValueTask<IActionResult> DeactivebyUser([Required] Guid userId)
    {
        try
        {
            return Ok(await _repository.DeactivebyUser(userId));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "DeactivebyUserUserController-Exception: {UserId}", userId);

            return Problem();
        }
    }
}

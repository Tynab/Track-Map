using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TrackMap.Api.Entities;
using TrackMap.Common.Requests;
using TrackMap.Common.Responses;
using YANLib;
using static System.DateTime;
using static System.Guid;

namespace TrackMap.Api.Controllers;

[Route("api/register")]
[ApiController]
public sealed class RegisterController : ControllerBase
{
    private readonly ILogger<RegisterController> _logger;
    private readonly UserManager<User> _userManager;

    public RegisterController(ILogger<RegisterController> logger, UserManager<User> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    [HttpPost]
    public async Task<IActionResult> Register([Required] RegisterRequest request)
    {
        try
        {
            var id = NewGuid();

            var result = await _userManager.CreateAsync(new User
            {
                Id = id,
                CreatedBy = id,
                CreatedAt = Now,
                IsActive = true,
                SecurityStamp = NewGuid().ToString(),
                UserName = request.UserName,
                NormalizedUserName = request.UserName.ToUpperInvariant()
            }, request.Password);

            return Ok(result.Succeeded
                ? new RegisterRepsonse
                {
                    Success = true
                }
                : new RegisterRepsonse
                {
                    Success = false,
                    Errors = result.Errors.Select(x => x.Description)
                });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "RegisterRegisterController-Exception: {Request}", request.CamelSerialize());

            return Problem();
        }
    }
}

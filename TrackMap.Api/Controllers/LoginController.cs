using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TrackMap.Api.Entities;
using TrackMap.Common.Requests;
using TrackMap.Common.Responses;
using YANLib;
using static Microsoft.IdentityModel.Tokens.SecurityAlgorithms;
using static System.DateTime;
using static System.Security.Claims.ClaimTypes;
using static System.Text.Encoding;

namespace TrackMap.Api.Controllers;

[Route("api/login")]
[ApiController]
public sealed class LoginController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IConfiguration _configuration;
    private readonly SignInManager<User> _signInManager;

    public LoginController(ILogger<UserController> logger, IConfiguration configuration, SignInManager<User> signInManager)
    {
        _logger = logger;
        _configuration = configuration;
        _signInManager = signInManager;
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Login")]
    public async ValueTask<IActionResult> Login([Required] LoginRequest request)
    {
        try
        {
            return (await _signInManager.PasswordSignInAsync(request.UserName!, request.Password!, false, false)).Succeeded
                ? Ok(new LoginResponse
                {
                    Success = true,
                    Token = new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(_configuration["JwtIssuer"], _configuration["JwtAudience"], new[]
                    {
                        new Claim(Name, request.UserName!)
                    }, expires: Now.AddDays(_configuration["JwtExpiryInDays"]!.ToInt(1)), signingCredentials: new SigningCredentials(new SymmetricSecurityKey(UTF8.GetBytes(_configuration["JwtSecurityKey"] ?? string.Empty)), HmacSha256)))
                })
                : BadRequest(new LoginResponse
                {
                    Success = false,
                    Error = "Username and Password are invalid"
                });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "LoginLoginController-Exception: {Request}", request.CamelSerialize());

            return Problem();
        }
    }
}

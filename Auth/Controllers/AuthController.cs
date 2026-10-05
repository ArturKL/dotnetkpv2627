using System.Security.Claims;
using Auth.Dtos;
using Auth.Dtos.Validators;
using Auth.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    [HttpPost("signup")]
    public async Task<IActionResult> Signup([FromBody] SignupDto dto, SignupDtoValidator validator, IAuthService authService)
    {
        var result = await validator.ValidateAsync(dto);
        if (!result.IsValid)
        {
            return BadRequest(result.Errors);
        }
        
        var user = await authService.SignupAsync(dto.Username, dto.Password);
        if (user == null)
        {
            return BadRequest("User already exists");
        }

        return Created();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto, LoginDtoValidator validator, IAuthService authService)
    {
        var result = await validator.ValidateAsync(dto);
        if (!result.IsValid)
        {
            return BadRequest(result.Errors);
        }
        
        var user = await authService.LoginAsync(dto.Username, dto.Password);
        if (user == null)
        {
            return Unauthorized("Invalid username or password");
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Username)
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return Ok();
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok();
    }
}
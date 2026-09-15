using Auth.Dtos;
using Auth.Models;
using Auth.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Controllers;

[ApiController]
[Route("users")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpGet("{username}")]
    public async Task<IActionResult> Get(string username)
    {
        var user = await userService.GetUserAsync(username);
        if (user == null)
        {
            return NotFound();
        }
        
        return Ok(new UserDto(user.Username, user.Description));
    }

    [HttpPut("{username}")]
    [Authorize]
    public async Task<IActionResult> Update(string username, UpdateUserDto dto)
    {
        var currentUser = User.Identity?.Name;
        if (currentUser == null || currentUser != username)
        {
            return Unauthorized();
        }

        var user = await userService.UpdateAsync(username, dto);
        if (user == null)
        {
            return NotFound();
        }
        
        return Ok(new UserDto(user.Username, user.Description));
    }

    [HttpDelete("{username}")]
    [Authorize]
    public async Task<IActionResult> Delete(string username)
    {
        var currentUser = User.Identity?.Name;
        if (currentUser == null || currentUser != username)
        {
            return Unauthorized();
        }

        var result = await userService.DeleteAsync(username);
        if (!result)
        {
            return BadRequest();
        }
        
        return Ok();
    }
}
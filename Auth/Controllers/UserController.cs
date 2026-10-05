using Auth.Dtos;
using Auth.Dtos.Validators;
using Auth.Models;
using Auth.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Controllers;

[ApiController]
[Route("users")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> SearchUsers(SearchUsersDto dto, SearchUsersDtoValidator validator)
    {
        var result = await validator.ValidateAsync(dto);
        if (!result.IsValid)
        {
            return BadRequest(result.Errors);
        }

        var users = await userService.SearchUsersAsync(dto);
        
        return Ok(users);
    }

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
    public async Task<IActionResult> Update(string username, UpdateUserDto dto, UpdateUserDtoValidator validator)
    {
        var result = await validator.ValidateAsync(dto);
        if (!result.IsValid)
        {
            return BadRequest(result.Errors);
        }

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
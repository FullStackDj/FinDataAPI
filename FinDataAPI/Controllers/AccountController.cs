using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FinDataAPI.DTOs.Account;
using FinDataAPI.Models;

namespace FinDataAPI.Controllers;

[Route("api/account")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;

    public AccountController(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (string.IsNullOrEmpty(registerDTO.Password))
            {
                return BadRequest("Password cannot be null or empty.");
            }

            var appUser = new AppUser
            {
                UserName = registerDTO.UserName,
                Email = registerDTO.Email,
            };

            var createdUser = await _userManager.CreateAsync(appUser, registerDTO.Password);

            if (createdUser.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(appUser, "User");

                if (roleResult.Succeeded)
                {
                    return Ok("User created successfully");
                }

                return StatusCode(500, string.Join(", ", roleResult.Errors.Select(e => e.Description)));
            }

            return StatusCode(500, string.Join(", ", createdUser.Errors.Select(e => e.Description)));
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}
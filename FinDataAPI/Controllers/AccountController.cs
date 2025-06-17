using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FinDataAPI.DTOs.Account;
using FinDataAPI.Interfaces;
using FinDataAPI.Models;

namespace FinDataAPI.Controllers;

[Route("api/account")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenService _tokenService;

    public AccountController(UserManager<AppUser> userManager, ITokenService tokenService)

    {
        _userManager = userManager;
        _tokenService = tokenService;
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
                    return Ok(
                        new NewUserDTO
                        {
                            Username = appUser.UserName ?? throw new Exception("UserName is null"),
                            Email = appUser.Email ?? throw new Exception("Email is null"),
                            Token = _tokenService.CreateToken(appUser)
                        }
                    );
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
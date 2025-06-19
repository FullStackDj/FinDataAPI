using Microsoft.EntityFrameworkCore;
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
    private readonly SignInManager<AppUser> _signInManager;

    public AccountController(UserManager<AppUser> userManager, ITokenService tokenService,
        SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _signInManager = signInManager;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDTO loginDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDTO.Username.ToLower());

        if (user == null)
        {
            return Unauthorized("Invalid username");
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);

        if (!result.Succeeded)
        {
            return Unauthorized("Username or password is incorrect");
        }

        return Ok(
            new NewUserDTO
            {
                Username = user.UserName ?? throw new Exception("UserName is null"),
                Email = user.Email ?? throw new Exception("Email is null"),
                Token = await _tokenService.CreateToken(user)
            }
        );
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
                            Token = await _tokenService.CreateToken(appUser)
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
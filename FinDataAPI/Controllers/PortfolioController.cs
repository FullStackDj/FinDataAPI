using FinDataAPI.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FinDataAPI.Interfaces;
using FinDataAPI.Models;

namespace FinDataAPI.Controllers;

[Route("api/portfolio")]
[ApiController]
public class PortfolioController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IStockRepository _stockRepo;
    private readonly IPortfolioRepository _portfolioRepo;

    public PortfolioController(UserManager<AppUser> userManager, IStockRepository stockRepo,
        IPortfolioRepository portfolioRepo)
    {
        _userManager = userManager;
        _stockRepo = stockRepo;
        _portfolioRepo = portfolioRepo;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUserPortfolio()
    {
        var username = User.GetUsername();

        if (string.IsNullOrEmpty(username))
        {
            return BadRequest("Username claim is missing.");
        }

        var appUser = await _userManager.FindByNameAsync(username);

        if (appUser == null)
        {
            return NotFound("User not found.");
        }

        var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);
        return Ok(userPortfolio);
    }
}
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FinDataAPI.Interfaces;
using FinDataAPI.Models;
using FinDataAPI.Extensions;

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

        if (username == null || string.IsNullOrEmpty(username))
        {
            return BadRequest("Username claim is missing.");
        }

        var appUser = await _userManager.FindByNameAsync(username);

        if (appUser == null)
        {
            return NotFound("User not found.");
        }

        var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser!);
        return Ok(userPortfolio);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddPortfolio(string symbol)
    {
        var username = User.GetUsername();
        var appUser = await _userManager.FindByNameAsync(username);
        var stock = await _stockRepo.GetBySymbolAsync(symbol);

        if (stock == null)
        {
            return BadRequest("Stock not found.");
        }

        var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);

        if (userPortfolio.Any(e => e.Symbol.ToLower() == symbol.ToLower()))
        {
            return BadRequest("Cannot add same symbol.");
        }

        var portfolioModel = new Portfolio
        {
            StockId = stock.Id,
            AppUserId = appUser.Id,
        };

        await _portfolioRepo.CreateAsync(portfolioModel);

        if (portfolioModel == null)
        {
            return StatusCode(500, "Could not create portfolio.");
        }

        return Created();
    }
}
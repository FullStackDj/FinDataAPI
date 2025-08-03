using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using FinDataAPI.Controllers;
using FinDataAPI.Interfaces;
using FinDataAPI.Models;

namespace FinDataAPI.Tests;

public class PortfolioControllerTests
{
    private readonly Mock<UserManager<AppUser>> _userManagerMock;
    private readonly Mock<IStockRepository> _stockRepoMock;
    private readonly Mock<IPortfolioRepository> _portfolioRepoMock;
    private readonly Mock<IFMPService> _fmpServiceMock;
    private readonly PortfolioController _controller;

    public PortfolioControllerTests()
    {
        var userStoreMock = new Mock<IUserStore<AppUser>>();
        _userManagerMock =
            new Mock<UserManager<AppUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
        _stockRepoMock = new Mock<IStockRepository>();
        _portfolioRepoMock = new Mock<IPortfolioRepository>();
        _fmpServiceMock = new Mock<IFMPService>();

        _controller = new PortfolioController(
            _userManagerMock.Object,
            _stockRepoMock.Object,
            _portfolioRepoMock.Object,
            _fmpServiceMock.Object);

        var user = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, "user") }, "mock"));
        _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
    }

    [Fact]
    public async Task GetPortfolioOk()
    {
        var appUser = new AppUser { UserName = "user", Id = "1" };
        var stocks = new List<Stock> { new Stock { Symbol = "TSLA" } };

        _userManagerMock.Setup(u => u.FindByNameAsync("user")).ReturnsAsync(appUser);
        _portfolioRepoMock.Setup(p => p.GetUserPortfolio(It.IsAny<AppUser>())).ReturnsAsync(stocks);

        var result = await _controller.GetUserPortfolio();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedStocks = Assert.IsAssignableFrom<IEnumerable<Stock>>(okResult.Value);
        Assert.Single(returnedStocks);
    }

    [Fact]
    public async Task AddPortfolioSuccess ()
    {
        var appUser = new AppUser { UserName = "user", Id = "1" };
        var stock = new Stock { Id = 1, Symbol = "TSLA" };

        _userManagerMock.Setup(u => u.FindByNameAsync("user")).ReturnsAsync(appUser);
        _stockRepoMock.Setup(s => s.GetBySymbolAsync("TSLA")).ReturnsAsync(stock);
        _portfolioRepoMock.Setup(p => p.GetUserPortfolio(It.IsAny<AppUser>())).ReturnsAsync(new List<Stock>());
        _portfolioRepoMock.Setup(p => p.CreateAsync(It.IsAny<Portfolio>()))
            .ReturnsAsync(new Portfolio { AppUserId = appUser.Id, StockId = stock.Id });

        var result = await _controller.AddPortfolio("TSLA");

        Assert.IsType<CreatedResult>(result);
    }

    [Fact]
    public async Task AddPortfolioBadRequest()
    {
        var appUser = new AppUser { UserName = "user", Id = "1" };

        _userManagerMock.Setup(u => u.FindByNameAsync("user")).ReturnsAsync(appUser);
        _stockRepoMock.Setup(s => s.GetBySymbolAsync("XYZ")).ReturnsAsync((Stock)null);
        _fmpServiceMock.Setup(f => f.FindStockBySymbolAsync("XYZ")).ReturnsAsync((Stock)null);

        var result = await _controller.AddPortfolio("XYZ");

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task DeletePortfolioOk()
    {
        var appUser = new AppUser { UserName = "user", Id = "1" };
        var stocks = new List<Stock> { new Stock { Symbol = "TSLA" } };

        _userManagerMock.Setup(u => u.FindByNameAsync("user")).ReturnsAsync(appUser);
        _portfolioRepoMock.Setup(p => p.GetUserPortfolio(It.IsAny<AppUser>())).ReturnsAsync(stocks);
        _portfolioRepoMock.Setup(p => p.DeletePortfolio(It.IsAny<AppUser>(), "TSLA"))
            .ReturnsAsync(new Portfolio { AppUserId = appUser.Id, StockId = 1 });

        var result = await _controller.DeletePortfolio("TSLA");

        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task DeletePortfolioBadRequest()
    {
        var appUser = new AppUser { UserName = "user", Id = "1" };

        _userManagerMock.Setup(u => u.FindByNameAsync("user")).ReturnsAsync(appUser);
        _portfolioRepoMock.Setup(p => p.GetUserPortfolio(It.IsAny<AppUser>())).ReturnsAsync(new List<Stock>());

        var result = await _controller.DeletePortfolio("TSLA");

        Assert.IsType<BadRequestObjectResult>(result);
    }
}
using Microsoft.AspNetCore.Mvc;
using Moq;
using FinDataAPI.Controllers;
using FinDataAPI.DTOs.Stock;
using FinDataAPI.Helpers;
using FinDataAPI.Interfaces;
using FinDataAPI.Models;

namespace FinDataAPI.Tests;

public class StockControllerTests
{
    private readonly Mock<IStockRepository> _repoMock;
    private readonly StockController _controller;

    public StockControllerTests()
    {
        _repoMock = new Mock<IStockRepository>();
        _controller = new StockController(null, _repoMock.Object);
    }

    [Fact]
    public async Task GetAllOk()
    {
        _repoMock.Setup(r => r.GetAllAsync(It.IsAny<QueryObject>()))
            .ReturnsAsync(new List<Stock> { new Stock { Id = 1, Symbol = "TSLA", CompanyName = "Tesla" } });

        var result = await _controller.GetAll(new());

        var okResult = Assert.IsType<OkObjectResult>(result);
        var resultList = Assert.IsType<List<StockDTO>>(okResult.Value);
        Assert.Single(resultList);
    }

    [Fact]
    public async Task GetByIdOk()
    {
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Stock { Id = 1, Symbol = "TSLA" });

        var result = await _controller.GetById(1);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetByIdNotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Stock)null);

        var result = await _controller.GetById(99);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task CreateOk()
    {
        var dto = new CreateStockRequestDTO { Symbol = "TSLA" };
        _repoMock.Setup(r => r.CreateAsync(It.IsAny<Stock>())).ReturnsAsync(new Stock { Id = 1, Symbol = "TSLA" });

        var result = await _controller.Create(dto);

        Assert.IsType<CreatedAtActionResult>(result);
    }

    [Fact]
    public async Task UpdateOk()
    {
        _repoMock.Setup(r => r.UpdateAsync(1, It.IsAny<UpdateStockRequestDTO>()))
            .ReturnsAsync(new Stock { Id = 1, Symbol = "TSLA" });

        var result = await _controller.Update(1, new UpdateStockRequestDTO());

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task UpdateNotFound()
    {
        _repoMock.Setup(r => r.UpdateAsync(99, It.IsAny<UpdateStockRequestDTO>())).ReturnsAsync((Stock)null);

        var result = await _controller.Update(99, new UpdateStockRequestDTO());

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteOk()
    {
        _repoMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(new Stock { Id = 1 });

        var result = await _controller.Delete(1);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteNotFound()
    {
        _repoMock.Setup(r => r.DeleteAsync(99)).ReturnsAsync((Stock)null);

        var result = await _controller.Delete(99);

        Assert.IsType<NotFoundResult>(result);
    }
}
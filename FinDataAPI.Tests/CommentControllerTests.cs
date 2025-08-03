using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using FinDataAPI.Controllers;
using FinDataAPI.DTOs.Comment;
using FinDataAPI.Helpers;
using FinDataAPI.Interfaces;
using FinDataAPI.Models;

namespace FinDataAPI.Tests;

public class CommentControllerTests
{
    private readonly Mock<ICommentRepository> _commentRepoMock;
    private readonly Mock<IStockRepository> _stockRepoMock;
    private readonly Mock<IFMPService> _fmpServiceMock;
    private readonly CommentController _controller;

    public CommentControllerTests()
    {
        _commentRepoMock = new Mock<ICommentRepository>();
        _stockRepoMock = new Mock<IStockRepository>();

        var userStoreMock = new Mock<IUserStore<AppUser>>();
        var userManagerMock =
            new Mock<UserManager<AppUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

        _fmpServiceMock = new Mock<IFMPService>();

        _controller = new CommentController(
            _commentRepoMock.Object,
            _stockRepoMock.Object,
            userManagerMock.Object,
            _fmpServiceMock.Object
        );

        var user = new ClaimsPrincipal(new ClaimsIdentity([
            new Claim(ClaimTypes.Name, "username1")
        ], "mock"));

        _controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = user }
        };
    }

    [Fact]
    public async Task GetAllReturnsOk()
    {
        _commentRepoMock.Setup(c => c.GetAllAsync(It.IsAny<CommentQueryObject>()))
            .ReturnsAsync(new List<Comment>());

        var result = await _controller.GetAll(new CommentQueryObject());

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetByIdNotFound()
    {
        _commentRepoMock.Setup(c => c.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Comment?)null);

        var result = await _controller.GetById(1);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task CreateBadRequest()
    {
        _stockRepoMock.Setup(s => s.GetBySymbolAsync(It.IsAny<string>())).ReturnsAsync((Stock?)null);
        _fmpServiceMock.Setup(f => f.FindStockBySymbolAsync(It.IsAny<string>()))!.ReturnsAsync((Stock?)null);

        var result = await _controller.Create("AAA", new CreateCommentDTO());

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Stock doesn't  exist", badRequestResult.Value);
    }

    [Fact]
    public async Task UpdateNotFound()
    {
        _commentRepoMock.Setup(c => c.UpdateAsync(It.IsAny<int>(), It.IsAny<Comment>()))
            .ReturnsAsync((Comment?)null);

        var result = await _controller.Update(1, new UpdateCommentRequestDTO());

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Comment not found", notFoundResult.Value);
    }

    [Fact]
    public async Task DeleteNotFound()
    {
        _commentRepoMock.Setup(c => c.DeleteAsync(It.IsAny<int>())).ReturnsAsync((Comment?)null);

        var result = await _controller.Delete(1);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Comment don't exist", notFoundResult.Value);
    }
}
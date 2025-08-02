using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using FinDataAPI.Controllers;
using FinDataAPI.DTOs.Account;
using FinDataAPI.Interfaces;
using FinDataAPI.Models;

namespace FinDataAPI.Tests;

public class AccountControllerTests
{
    private readonly Mock<UserManager<AppUser>> _userManagerMock;
    private readonly Mock<SignInManager<AppUser>> _signInManagerMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly AccountController _controller;

    public AccountControllerTests()
    {
        var userStoreMock = new Mock<IUserStore<AppUser>>();
        _userManagerMock = new Mock<UserManager<AppUser>>(
            userStoreMock.Object, null, null, null, null, null, null, null, null
        );

        var contextAccessorMock = new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
        var userPrincipalFactoryMock = new Mock<IUserClaimsPrincipalFactory<AppUser>>();

        _signInManagerMock = new Mock<SignInManager<AppUser>>(
            _userManagerMock.Object,
            contextAccessorMock.Object,
            userPrincipalFactoryMock.Object,
            null, null, null, null
        );

        _tokenServiceMock = new Mock<ITokenService>();

        _controller = new AccountController(
            _userManagerMock.Object,
            _tokenServiceMock.Object,
            _signInManagerMock.Object
        );
    }

    [Fact]
    public async Task LoginSuccess()
    {
        var user = new AppUser { UserName = "username1", Email = "email1@email.com" };
        var loginDto = new LoginDTO { Username = "username1", Password = "password2" };

        _userManagerMock.Setup(um => um.Users)
            .Returns(new List<AppUser> { user }.AsQueryable());

        _signInManagerMock
            .Setup(sm => sm.CheckPasswordSignInAsync(user, loginDto.Password, false))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

        _tokenServiceMock.Setup(ts => ts.CreateToken(user))
            .ReturnsAsync("mock-token");

        var result = await _controller.Login(loginDto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedUser = Assert.IsType<NewUserDTO>(okResult.Value);
        Assert.Equal("username1", returnedUser.UserName);
        Assert.Equal("email1@email.com", returnedUser.Email);
        Assert.Equal("mock-token", returnedUser.Token);
    }

    [Fact]
    public async Task RegisterSuccess()
    {
        var registerDto = new RegisterDTO
        {
            UserName = "username1",
            Email = "email1@email.com",
            Password = "password123"
        };


        _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<AppUser>(), registerDto.Password))
            .ReturnsAsync(IdentityResult.Success);

        _userManagerMock.Setup(um => um.AddToRoleAsync(It.IsAny<AppUser>(), "User"))
            .ReturnsAsync(IdentityResult.Success);

        _tokenServiceMock.Setup(ts => ts.CreateToken(It.IsAny<AppUser>()))
            .ReturnsAsync("register-token");

        var result = await _controller.Register(registerDto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedUser = Assert.IsType<NewUserDTO>(okResult.Value);
        Assert.Equal("username1", returnedUser.UserName);
        Assert.Equal("email1@email.com", returnedUser.Email);
        Assert.Equal("register-token", returnedUser.Token);
    }
}
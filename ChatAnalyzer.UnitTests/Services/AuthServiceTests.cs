using System.Security.Authentication;
using ChatAnalyzer.Application.Services;
using ChatAnalyzer.Domain.Entities;
using ChatAnalyzer.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace ChatAnalyzer.UnitTests.Services;

public class AuthServiceTests
{
    private readonly AuthService _authService;
    private readonly Mock<SignInManager<ApplicationUser>> _signInManagerMock;
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;

    public AuthServiceTests()
    {
        _userManagerMock = new Mock<UserManager<ApplicationUser>>(
            new Mock<IUserStore<ApplicationUser>>().Object, null!, null!, null!, null!, null!, null!, null!, null!);

        _signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
            _userManagerMock.Object, new Mock<IHttpContextAccessor>().Object,
            new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object, null!, null!, null!, null!);

        var loggerMock = new Mock<ILogger<AuthService>>();
        _authService = new AuthService(_userManagerMock.Object, _signInManagerMock.Object, loggerMock.Object);
    }

    [Fact]
    public async Task RegisterAsync_WhenDataIsValid_ShouldRegisterUser()
    {
        _userManagerMock.Setup(um => um.Users)
            .Returns(new List<ApplicationUser>().AsQueryable());
        _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
        _signInManagerMock.Setup(sm =>
                sm.PasswordSignInAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), false, false))
            .ReturnsAsync(SignInResult.Success);

        await _authService.RegisterAsync("testuser", "test@example.com", "Password123!");

        _userManagerMock.Verify(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
        _signInManagerMock.Verify(
            sm => sm.PasswordSignInAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), false, false), Times.Once);
    }

    [Fact]
    public async Task RegisterAsync_WhenUsernameIsTaken_ShouldThrowException()
    {
        _userManagerMock.Setup(um => um.Users)
            .Returns(new List<ApplicationUser> { new() { UserName = "testuser" } }.AsQueryable());

        await Assert.ThrowsAsync<UsernameAlreadyTakenException>(() =>
            _authService.RegisterAsync("testuser", "test@example.com", "Password123!"));
    }

    [Fact]
    public async Task LoginAsync_WhenUserNotFound_ShouldThrowException()
    {
        _userManagerMock.Setup(um => um.Users)
            .Returns(new List<ApplicationUser>().AsQueryable());

        await Assert.ThrowsAsync<UserNotFoundException>(() =>
            _authService.LoginAsync("test@example.com", "Password123!"));
    }

    [Fact]
    public async Task LoginAsync_WhenPasswordIsIncorrect_ShouldThrowException()
    {
        var user = new ApplicationUser { Email = "test@example.com" };
        _userManagerMock.Setup(um => um.Users)
            .Returns(new List<ApplicationUser> { user }.AsQueryable());
        _signInManagerMock.Setup(sm => sm.PasswordSignInAsync(user, It.IsAny<string>(), true, false))
            .ReturnsAsync(SignInResult.Failed);

        await Assert.ThrowsAsync<InvalidCredentialException>(() =>
            _authService.LoginAsync("test@example.com", "WrongPassword!"));
    }

    [Fact]
    public async Task LogoutAsync_WhenCalled_ShouldCallSignOut()
    {
        _signInManagerMock.Setup(sm => sm.SignOutAsync()).Returns(Task.CompletedTask);

        await _authService.LogoutAsync();

        _signInManagerMock.Verify(sm => sm.SignOutAsync(), Times.Once);
    }
}
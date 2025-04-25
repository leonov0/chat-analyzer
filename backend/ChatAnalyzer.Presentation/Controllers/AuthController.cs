using System.Security.Authentication;
using System.Security.Claims;
using ChatAnalyzer.Application.Interfaces;
using ChatAnalyzer.Domain.Exceptions;
using ChatAnalyzer.Presentation.Requests;
using ChatAnalyzer.Presentation.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatAnalyzer.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService, IUserService userService, ILogger<AuthController> logger)
    : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            await authService.RegisterAsync(registerDto.Username, registerDto.Email, registerDto.Password);

            return Created();
        }
        catch (EmailTakenException ex)
        {
            return Conflict(new { Title = ex.Message });
        }
        catch (UsernameAlreadyTakenException ex)
        {
            return Conflict(new { Title = ex.Message });
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { Title = "An error occurred during registration. Please try again later." });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        try
        {
            await authService.LoginAsync(loginDto.Email, loginDto.Password);

            return Ok();
        }
        catch (InvalidCredentialException)
        {
            return Unauthorized(new { Title = "Invalid email or password." });
        }
        catch (UserNotFoundException)
        {
            return Unauthorized(new { Title = "Invalid email or password." });
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { Title = "An error occurred during login. Please try again later." });
        }
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await authService.LogoutAsync();

        return Ok();
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        try
        {
            var user = await userService.GetByIdAsync(userId);

            if (user == null)
            {
                logger.LogError("Current user not found. User ID: {userId}", userId);
                return NotFound(new { Title = "User not found." });
            }

            if (user.UserName == null) logger.LogError("Current user has no username. User ID: {userId}", userId);

            if (user.Email == null) logger.LogError("Current user has no email. User ID: {userId}", userId);

            return Ok(new CurrentUserDto
            {
                Id = user.Id,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty
            });
        }
        catch (UserNotFoundException)
        {
            return NotFound(new { Title = "User not found." });
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { Title = "An error occurred while retrieving user information. Please try again later." });
        }
    }
}
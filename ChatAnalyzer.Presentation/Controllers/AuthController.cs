using System.Security.Authentication;
using ChatAnalyzer.Application.Interfaces;
using ChatAnalyzer.Domain.Exceptions;
using ChatAnalyzer.Presentation.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatAnalyzer.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
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
}
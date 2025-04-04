using System.Security.Authentication;
using ChatAnalyzer.Application.Interfaces;
using ChatAnalyzer.Domain.Entities;
using ChatAnalyzer.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ChatAnalyzer.Application.Services;

public class AuthService(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    ILogger<AuthService> logger)
    : IAuthService
{
    public async Task RegisterAsync(string username, string email, string password)
    {
        var isUsernameTaken = userManager.Users.Any(u => u.UserName == username);

        if (isUsernameTaken) throw new UsernameAlreadyTakenException(username);

        var isEmailTaken = userManager.Users.Any(u => u.Email == email);

        if (isEmailTaken) throw new EmailTakenException(email);

        var user = new ApplicationUser
        {
            UserName = username,
            Email = email
        };

        var result = await userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            var errors = string.Join(". ", result.Errors.Select(error => error.Description));
            logger.LogError("Registration failed for user {user}: {errors}.", user, errors);
            throw new Exception("Registration failed.");
        }

        await signInManager.PasswordSignInAsync(user, password, false, false);
    }

    public async Task LoginAsync(string email, string password)
    {
        var user = userManager.Users.FirstOrDefault(u => u.Email == email);

        if (user == null) throw new UserNotFoundException(email);

        var result = await signInManager.PasswordSignInAsync(user, password, true, false);

        if (!result.Succeeded) throw new InvalidCredentialException();
    }

    public async Task LogoutAsync()
    {
        await signInManager.SignOutAsync();
    }
}
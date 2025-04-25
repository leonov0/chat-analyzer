using ChatAnalyzer.Application.Interfaces;
using ChatAnalyzer.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace ChatAnalyzer.Application.Services;

public class UserService(UserManager<ApplicationUser> userManager) : IUserService
{
    public async Task<ApplicationUser?> GetByIdAsync(Guid id)
    {
        var user = await userManager.FindByIdAsync(id.ToString());

        return user;
    }
}
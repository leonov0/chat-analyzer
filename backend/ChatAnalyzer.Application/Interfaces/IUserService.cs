using ChatAnalyzer.Domain.Entities;

namespace ChatAnalyzer.Application.Interfaces;

public interface IUserService
{
    Task<ApplicationUser?> GetByIdAsync(Guid id);
}
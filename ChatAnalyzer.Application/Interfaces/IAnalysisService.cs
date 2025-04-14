using ChatAnalyzer.Domain.Entities;

namespace ChatAnalyzer.Application.Interfaces;

public interface IAnalysisService
{
    Task<Analysis> CreateAsync(Chat chat, Guid userId);
    Task<Analysis?> GetByIdAsync(Guid id);
    Task<IEnumerable<Analysis>> GetAllAsync(Guid userId);
}
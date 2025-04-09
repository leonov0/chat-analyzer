using ChatAnalyzer.Domain.Entities;

namespace ChatAnalyzer.Domain.Interfaces;

public interface IAnalysisRepository
{
    Task<Analysis> CreateAsync(ChatHistory chatHistory, Guid userId);
    Task<Analysis?> GetByIdAsync(Guid id);
    Task<IEnumerable<Analysis>> GetAllAsync(Guid userId);
}
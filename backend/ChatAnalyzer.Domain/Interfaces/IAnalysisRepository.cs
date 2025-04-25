using ChatAnalyzer.Domain.Entities;

namespace ChatAnalyzer.Domain.Interfaces;

public interface IAnalysisRepository
{
    Task CreateAsync(Analysis analysis);
    Task<Analysis?> GetByIdAsync(Guid id);
    Task<IEnumerable<Analysis>> GetAllAsync(Guid userId);
}
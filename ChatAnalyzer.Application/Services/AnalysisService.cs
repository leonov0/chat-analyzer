using ChatAnalyzer.Application.Interfaces;
using ChatAnalyzer.Domain.Entities;
using ChatAnalyzer.Domain.Interfaces;

namespace ChatAnalyzer.Application.Services;

public class AnalysisService(IAnalysisRepository repository) : IAnalysisService
{
    public async Task<Analysis> CreateAsync(ChatHistory chatHistory, Guid userId)
    {
        var analysis = new Analysis
        {
            Name = chatHistory.Name,
            UserId = userId
        };

        await repository.CreateAsync(analysis);

        return analysis;
    }

    public async Task<Analysis?> GetByIdAsync(Guid id)
    {
        var analysis = await repository.GetByIdAsync(id);

        return analysis;
    }

    public async Task<IEnumerable<Analysis>> GetAllAsync(Guid userId)
    {
        var analyses = await repository.GetAllAsync(userId);

        return analyses;
    }
}
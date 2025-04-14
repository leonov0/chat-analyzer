using ChatAnalyzer.Application.Interfaces;
using ChatAnalyzer.Domain.Entities;
using ChatAnalyzer.Domain.Interfaces;

namespace ChatAnalyzer.Application.Services;

public class AnalysisService(IAnalysisRepository repository, IAnalyzer analyzer) : IAnalysisService
{
    public async Task<Analysis> CreateAsync(Chat chat, Guid userId)
    {
        var analysisResult = await analyzer.Analyze(chat);

        var analysis = new Analysis
        {
            Name = chat.Name,
            UserId = userId,
            Messages = [new AnalysisMessage { Content = analysisResult }]
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
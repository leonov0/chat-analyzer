using ChatAnalyzer.Domain.Entities;

namespace ChatAnalyzer.Domain.Interfaces;

public interface IAnalysisMessageRepository
{
    Task CreateAsync(AnalysisMessage analysis);
}
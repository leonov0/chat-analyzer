using ChatAnalyzer.Domain.Entities;
using ChatAnalyzer.Domain.Interfaces;
using ChatAnalyzer.Infrastructure.Persistence;

namespace ChatAnalyzer.Infrastructure.Repositories;

public class AnalysisMessageRepository(ApplicationDbContext dbContext) : IAnalysisMessageRepository
{
    public async Task CreateAsync(AnalysisMessage analysis)
    {
        await dbContext.AnalysisMessages.AddAsync(analysis);
        await dbContext.SaveChangesAsync();
    }
}
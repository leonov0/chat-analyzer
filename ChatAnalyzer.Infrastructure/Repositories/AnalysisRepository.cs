using ChatAnalyzer.Domain.Entities;
using ChatAnalyzer.Domain.Interfaces;
using ChatAnalyzer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ChatAnalyzer.Infrastructure.Repositories;

public class AnalysisRepository(ApplicationDbContext dbContext) : IAnalysisRepository
{
    public async Task<Analysis> CreateAsync(ChatHistory chatHistory, Guid userId)
    {
        // TODO: Implement logic to analyze the chat history and generate messages.
        AnalysisMessage welcomeMessage =
            new()
            {
                Id = Guid.NewGuid(),
                Content = "Welcome to ChatAnalyzer! This is a sample message."
            };

        List<AnalysisMessage> messages = [welcomeMessage];

        var analysis = new Analysis
        {
            Id = Guid.NewGuid(),
            Name = chatHistory.Name,
            UserId = userId,
            AnalysisMessages = messages
        };

        await dbContext.Analyses.AddAsync(analysis);
        await dbContext.SaveChangesAsync();

        return analysis;
    }

    public async Task<Analysis?> GetByIdAsync(Guid id)
    {
        var analysis = await dbContext.Analyses
            .Include(a => a.AnalysisMessages)
            .FirstOrDefaultAsync(a => a.Id == id);

        return analysis;
    }

    public Task<IEnumerable<Analysis>> GetAllAsync(Guid userId)
    {
        var analyses = dbContext.Analyses
            .Where(a => a.UserId == userId)
            .AsEnumerable();

        return Task.FromResult(analyses);
    }
}
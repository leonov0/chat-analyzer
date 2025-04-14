﻿using ChatAnalyzer.Domain.Entities;
using ChatAnalyzer.Domain.Interfaces;
using ChatAnalyzer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ChatAnalyzer.Infrastructure.Repositories;

public class AnalysisRepository(ApplicationDbContext dbContext) : IAnalysisRepository
{
    public async Task<Analysis?> GetByIdAsync(Guid id)
    {
        var analysis = await dbContext.Analyses
            .Include(a => a.Messages)
            .FirstOrDefaultAsync(a => a.Id == id);

        return analysis;
    }

    public async Task<IEnumerable<Analysis>> GetAllAsync(Guid userId)
    {
        var analyses = await dbContext.Analyses
            .Where(a => a.UserId == userId)
            .ToListAsync();

        return analyses;
    }

    public async Task UpdateAsync(Analysis analysis)
    {
        dbContext.Analyses.Update(analysis);

        await dbContext.SaveChangesAsync();
    }

    public async Task CreateAsync(Analysis analysis)
    {
        await dbContext.Analyses.AddAsync(analysis);
        await dbContext.SaveChangesAsync();
    }
}
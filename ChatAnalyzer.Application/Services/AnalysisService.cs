﻿using System.Text.Json;
using ChatAnalyzer.Application.Interfaces;
using ChatAnalyzer.Domain.Entities;
using ChatAnalyzer.Domain.Interfaces;

namespace ChatAnalyzer.Application.Services;

public class AnalysisService(IAnalysisRepository repository, IAnalyzer analyzer, ICryptoService cryptoService)
    : IAnalysisService
{
    public async Task<Analysis> CreateAsync(Chat chat, Guid userId)
    {
        var analysisResult = await analyzer.Analyze(chat);

        var analysis = new Analysis
        {
            Name = chat.Name,
            UserId = userId,
            Messages = [new AnalysisMessage { Content = analysisResult }],
            EncryptedChat = cryptoService.Encrypt(JsonSerializer.Serialize(chat))
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
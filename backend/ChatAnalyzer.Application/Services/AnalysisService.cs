using System.Text.Json;
using ChatAnalyzer.Application.Interfaces;
using ChatAnalyzer.Domain.Entities;
using ChatAnalyzer.Domain.Interfaces;

namespace ChatAnalyzer.Application.Services;

public class AnalysisService(
    IAnalysisRepository analysisRepository,
    IAnalyzer analyzer,
    ICryptoService cryptoService,
    IAnalysisMessageRepository analysisMessageRepository)
    : IAnalysisService
{
    public async Task<Analysis> CreateAsync(Chat chat, Guid userId)
    {
        var analysisResult = await analyzer.AnalyzeAsync(chat);

        var analysis = new Analysis
        {
            Name = chat.Name,
            UserId = userId,
            Messages = [new AnalysisMessage { Content = analysisResult, MessageType = MessageType.Received }],
            EncryptedChat = cryptoService.Encrypt(JsonSerializer.Serialize(chat))
        };

        await analysisRepository.CreateAsync(analysis);

        return analysis;
    }

    public async Task<Analysis?> GetByIdAsync(Guid id)
    {
        var analysis = await analysisRepository.GetByIdAsync(id);

        return analysis;
    }

    public async Task<IEnumerable<Analysis>> GetAllAsync(Guid userId)
    {
        var analyses = await analysisRepository.GetAllAsync(userId);

        return analyses;
    }

    public async Task<Analysis> AskAsync(Analysis analysis, string message)
    {
        var userMessage = new AnalysisMessage
        {
            AnalysisId = analysis.Id,
            Content = message,
            MessageType = MessageType.Sent
        };

        await analysisMessageRepository.CreateAsync(userMessage);

        var chat = JsonSerializer.Deserialize<Chat>(cryptoService.Decrypt(analysis.EncryptedChat));

        if (chat == null) throw new Exception("Chat not found.");

        var reply = await analyzer.AskAsync(chat, message, analysis);

        var replyMessage = new AnalysisMessage
        {
            AnalysisId = analysis.Id,
            Content = reply,
            MessageType = MessageType.Received
        };

        await analysisMessageRepository.CreateAsync(replyMessage);

        analysis.Messages.AddRange(userMessage, replyMessage);

        return analysis;
    }
}
using ChatAnalyzer.Application.Interfaces;
using ChatAnalyzer.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace ChatAnalyzer.Infrastructure.Services;

public class Analyzer(SemanticKernelService semanticKernelService, ILogger<Analyzer> logger) : IAnalyzer
{
    public async Task<string> Analyze(ChatHistory chatHistory)
    {
        var chatHistoryString = string.Join("\n", chatHistory.Messages
            .Select(m => $"{m.DateUnixtime}[{m.From}]: {m.TextEntities.Select(e => e.Text)}"));

        // TODO: Add a prompt to the chat history string

        var result = await semanticKernelService.AskAsync(chatHistoryString);

        if (!string.IsNullOrEmpty(result)) return result;

        logger.LogError("The result is empty. Chat history: {chatHistory}", chatHistoryString);
        return "Sorry, I couldn't analyze the chat history. Please try again later.";
    }
}
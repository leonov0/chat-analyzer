using ChatAnalyzer.Application.Interfaces;
using ChatAnalyzer.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace ChatAnalyzer.Infrastructure.Services;

public class Analyzer(SemanticKernelService semanticKernelService, ILogger<Analyzer> logger) : IAnalyzer
{
    // TODO: Improve the prompt to get better results
    private const string PromptHeader = "Analyze the following chat conversation. " +
                                        "Summarize the main topics discussed and the tone of the conversation. " +
                                        "Provide the summary in 3-5 sentences.\n";

    private const string FallbackMessage = "Sorry, I couldn't analyze the chat history. Please try again later.";

    public async Task<string> AnalyzeAsync(Chat chat)
    {
        var messages = string.Join("\n", chat.Messages
            .Select(m => $"{m.DateUnixtime} [{m.From}]: {string.Join(" ", m.TextEntities.Select(e => e.Text))}"));

        try
        {
            var result = await semanticKernelService.GenerateReplyAsync(PromptHeader + messages);

            if (!string.IsNullOrEmpty(result)) return result;

            logger.LogError("The result is empty. Chat: {chat}", messages);
            return FallbackMessage;
        }
        catch (Exception ex)
        {
            logger.LogError(
                "An error occurred while analyzing the chat history. Chat: {chat}. Exception: {chat}",
                messages, ex.Message);
            return FallbackMessage;
        }
    }

    public async Task<string> AskAsync(Chat chat, string message)
    {
        var messages = string.Join("\n", chat.Messages
            .Select(m => $"{m.DateUnixtime} [{m.From}]: {string.Join(" ", m.TextEntities.Select(e => e.Text))}"));

        try
        {
            var result = await semanticKernelService.GenerateReplyAsync(PromptHeader + messages + "\n" + message);

            if (!string.IsNullOrEmpty(result)) return result;

            logger.LogError("The result is empty. Chat: {chat}", messages);
            return FallbackMessage;
        }
        catch (Exception ex)
        {
            logger.LogError(
                "An error occurred while analyzing the chat history. Chat: {chat}. Exception: {chat}",
                messages, ex.Message);
            return FallbackMessage;
        }
    }
}
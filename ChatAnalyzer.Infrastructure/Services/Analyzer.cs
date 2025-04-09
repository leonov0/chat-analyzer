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

    public async Task<string> Analyze(ChatHistory chatHistory)
    {
        var messages = string.Join("\n", chatHistory.Messages
            .Select(m => $"{m.DateUnixtime} [{m.From}]: {string.Join(" ", m.TextEntities.Select(e => e.Text))}"));

        try
        {
            var result = await semanticKernelService.GenerateReplyAsync(PromptHeader + messages);

            if (!string.IsNullOrEmpty(result)) return result;

            logger.LogError("The result is empty. Chat history: {chatHistory}", messages);
            return FallbackMessage;
        }
        catch (Exception ex)
        {
            logger.LogError(
                "An error occurred while analyzing the chat history. Chat history: {chatHistory}. Exception: {exception}",
                messages, ex.Message);
            return FallbackMessage;
        }
    }
}
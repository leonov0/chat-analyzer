using ChatAnalyzer.Application.Interfaces;
using ChatAnalyzer.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace ChatAnalyzer.Infrastructure.Services;

public class Analyzer(SemanticKernelService semanticKernelService, ILogger<Analyzer> logger) : IAnalyzer
{
    private const string AnalyzePromptHeader = """
                                               You are a helpful assistant specialized in analyzing chat conversations.
                                               Please analyze the following chat history. Your task is to:
                                               1. Summarize the main topics discussed.
                                               2. Identify the overall tone (e.g., formal, casual, friendly, tense).
                                               3. Note any significant changes in tone or topic.
                                               Provide the summary in 3 to 5 concise sentences.

                                               Chat log:
                                               """;

    private const string AskPromptHeader = """
                                           You are a helpful assistant analyzing chat history.
                                           Below is a conversation log. Based on this context, please answer the user’s question that follows.

                                           Chat log:
                                           """;

    private const string FallbackMessage = "Sorry, I couldn't analyze the chat history. Please try again later.";

    public async Task<string> AnalyzeAsync(Chat chat)
    {
        var messages = string.Join("\n", chat.Messages
            .Select(m =>
            {
                var timestamp = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(m.DateUnixtime)).ToString("u");
                var content = string.Join(" ", m.TextEntities.Select(e => e.Text));
                return $"{timestamp} [{m.From}]: {content}";
            }));

        try
        {
            var result = await semanticKernelService.GenerateReplyAsync($"{AnalyzePromptHeader}\n{messages}");

            if (!string.IsNullOrEmpty(result))
                return result;

            logger.LogError("The result is empty. Chat: {chat}", messages);
            return FallbackMessage;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while analyzing the chat history. Chat: {chat}", messages);
            return FallbackMessage;
        }
    }

    public async Task<string> AskAsync(Chat chat, string message)
    {
        var messages = string.Join("\n", chat.Messages
            .Select(m =>
            {
                var timestamp = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(m.DateUnixtime)).ToString("u");
                var content = string.Join(" ", m.TextEntities.Select(e => e.Text));
                return $"{timestamp} [{m.From}]: {content}";
            }));

        var prompt = $"{AskPromptHeader}\n{messages}\n\nUser question: {message}";

        try
        {
            var result = await semanticKernelService.GenerateReplyAsync(prompt);

            if (!string.IsNullOrEmpty(result))
                return result;

            logger.LogError("The result is empty. Chat: {chat}", messages);
            return FallbackMessage;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while processing the question. Chat: {chat}", messages);
            return FallbackMessage;
        }
    }
}
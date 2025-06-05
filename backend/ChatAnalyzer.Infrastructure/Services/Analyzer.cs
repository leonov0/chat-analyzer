using System.Text;
using System.Text.Json;
using ChatAnalyzer.Application.Interfaces;
using ChatAnalyzer.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace ChatAnalyzer.Infrastructure.Services;

public class Analyzer(SemanticKernelService semanticKernelService, ILogger<Analyzer> logger)
    : IAnalyzer
{
    private const string AnalyzePromptTemplate = """
                                                 <context>
                                                 You are a helpful assistant specialized in analyzing chat conversations from a Telegram chat.
                                                 Your task is to:
                                                 1. Summarize the main topics discussed.
                                                 2. Identify the overall tone (e.g., formal, casual, friendly, tense).
                                                 3. Note any significant changes in tone or topic.
                                                 Provide the summary in 3 to 5 concise sentences.
                                                 </context>
                                                 <chat_log type="json">
                                                 {0}
                                                 </chat_log>
                                                 """;

    private const string AskPromptTemplate = """
                                             <context>
                                             You are a helpful assistant analyzing chat history from a Telegram chat.
                                             Based on the provided chat log and any previous questions and answers in this session, please answer the user’s follow-up question.
                                             </context>
                                             <chat_log type="json">
                                             {0}
                                             </chat_log>
                                             <qa_history>
                                             {1}
                                             </qa_history>
                                             <user_question>
                                             {2}
                                             </user_question>
                                             """;

    private const string FallbackMessage = "Sorry, I couldn't analyze the chat history. Please try again later.";

    private readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = false };
    private readonly ILogger<Analyzer> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    private readonly SemanticKernelService _semanticKernelService =
        semanticKernelService ?? throw new ArgumentNullException(nameof(semanticKernelService));

    public async Task<string> AnalyzeAsync(Chat chat)
    {
        var chatJson = JsonSerializer.Serialize(chat, _jsonOptions);
        var prompt = string.Format(AnalyzePromptTemplate, chatJson);

        try
        {
            var result = await _semanticKernelService.GenerateReplyAsync(prompt);

            if (!string.IsNullOrWhiteSpace(result))
                return result;

            _logger.LogError("Empty result from SemanticKernelService for chat ID: {ChatId}", chat.Id);
            return FallbackMessage;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during chat analysis for chat ID: {ChatId}", chat.Id);
            return FallbackMessage;
        }
    }

    public async Task<string> AskAsync(Chat chat, string message, Analysis analysis)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            _logger.LogWarning("Message is null or whitespace in AskAsync.");
            return FallbackMessage;
        }

        var chatJson = JsonSerializer.Serialize(chat, _jsonOptions);
        var qaHistoryText = BuildQaHistoryText(analysis.Messages);

        var prompt = string.Format(AskPromptTemplate, chatJson, qaHistoryText, message);

        try
        {
            var result = await _semanticKernelService.GenerateReplyAsync(prompt);

            if (!string.IsNullOrWhiteSpace(result))
                return result;

            _logger.LogError("Empty result from SemanticKernelService for chat ID: {ChatId}", chat.Id);
            return FallbackMessage;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error answering question for chat ID: {ChatId}", chat.Id);
            return FallbackMessage;
        }
    }

    private static string BuildQaHistoryText(List<AnalysisMessage> messages)
    {
        if (messages.Count == 0)
            return "No previous Q&A in this session.";

        var builder = new StringBuilder();
        foreach (var message in messages)
        {
            if (message.MessageType == MessageType.Sent)
            {
                builder.AppendLine($"User: {message.Content}");
                continue;
            }

            builder.AppendLine($"Assistant: {message.Content}");
        }

        return builder.ToString().Trim();
    }
}
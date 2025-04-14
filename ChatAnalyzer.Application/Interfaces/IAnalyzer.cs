using ChatAnalyzer.Domain.Entities;

namespace ChatAnalyzer.Application.Interfaces;

public interface IAnalyzer
{
    Task<string> AnalyzeAsync(Chat chat);
    Task<string> AskAsync(Chat chat, string message);
}
using ChatAnalyzer.Domain.Entities;

namespace ChatAnalyzer.Application.Interfaces;

public interface IAnalyzer
{
    Task<string> Analyze(Chat chat);
}
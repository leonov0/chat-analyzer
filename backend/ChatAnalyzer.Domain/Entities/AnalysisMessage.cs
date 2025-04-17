using System.ComponentModel.DataAnnotations;

namespace ChatAnalyzer.Domain.Entities;

public class AnalysisMessage
{
    [Key] public Guid Id { get; set; } = Guid.NewGuid();

    public string Content { get; set; } = string.Empty;

    public Guid AnalysisId { get; set; }
    public Analysis Analysis { get; set; } = null!;

    public MessageType MessageType { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
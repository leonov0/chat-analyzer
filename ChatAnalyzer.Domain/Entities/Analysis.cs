using System.ComponentModel.DataAnnotations;

namespace ChatAnalyzer.Domain.Entities;

public class Analysis
{
    [Key] public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = string.Empty;

    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;

    public List<AnalysisMessage> Messages { get; set; } = [];

    public string EncryptedChat { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
using System.ComponentModel.DataAnnotations;

namespace ChatAnalyzer.Domain.Entities;

public class Analysis
{
    [Key] public Guid Id { get; set; } = Guid.NewGuid();

    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;

    public List<AnalysisMessage> AnalysisMessages { get; set; } = [];

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
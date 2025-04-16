using System.Text.Json.Serialization;

namespace ChatAnalyzer.Presentation.Responses;

public class AnalysisMessageDto
{
    [JsonPropertyName("id")] public Guid Id { get; set; }
    [JsonPropertyName("content")] public string Content { get; set; } = string.Empty;
    [JsonPropertyName("created_at")] public DateTime CreatedAt { get; set; }
    [JsonPropertyName("updated_at")] public DateTime UpdatedAt { get; set; }
}
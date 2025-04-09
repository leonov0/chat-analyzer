using System.Text.Json.Serialization;

namespace ChatAnalyzer.Presentation.Responses;

public class AnalysisPreviewDto
{
    [JsonPropertyName("id")] public Guid Id { get; set; }
    [JsonPropertyName("created_at")] public DateTime CreatedAt { get; set; }
    [JsonPropertyName("updated_at")] public DateTime UpdatedAt { get; set; }
}
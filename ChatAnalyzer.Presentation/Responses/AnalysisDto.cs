using System.Text.Json.Serialization;

namespace ChatAnalyzer.Presentation.Responses;

public class AnalysisDto
{
    [JsonPropertyName("id")] public Guid Id { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; } = string.Empty;
    [JsonPropertyName("messages")] public IEnumerable<AnalysisMessageDto> Messages { get; set; }
    [JsonPropertyName("created_at")] public DateTime CreatedAt { get; set; }
    [JsonPropertyName("updated_at")] public DateTime UpdatedAt { get; set; }
}
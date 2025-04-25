using System.Text.Json.Serialization;

namespace ChatAnalyzer.Presentation.Responses;

public class CurrentUserDto
{
    [JsonPropertyName("id")] public Guid Id { get; set; }

    [JsonPropertyName("username")] public string UserName { get; set; } = string.Empty;

    [JsonPropertyName("email")] public string Email { get; set; } = string.Empty;
}
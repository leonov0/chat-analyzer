using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ChatAnalyzer.Presentation.Requests;

public class LoginDto
{
    [Required] [JsonPropertyName("email")] public string Email { get; set; } = string.Empty;

    [Required]
    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;
}
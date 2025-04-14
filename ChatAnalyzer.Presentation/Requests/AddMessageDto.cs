using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ChatAnalyzer.Presentation.Requests;

public class AddMessageDto
{
    [Required]
    [StringLength(1000, MinimumLength = 1, ErrorMessage = "Message must be between 1 and 1000 characters.")]
    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;
}
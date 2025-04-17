using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ChatAnalyzer.Presentation.Requests;

public class RegisterDto
{
    [Required]
    [MinLength(3)]
    [MaxLength(20)]
    [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Username can only contain letters and numbers.")]
    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;

    [Required] [EmailAddress] public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*\W).+$",
        ErrorMessage =
            "The password must contain at least one non-alphanumeric character, one digit, and one uppercase letter.")]
    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
    [JsonPropertyName("password_confirmation")]

    public string PasswordConfirmation { get; set; } = string.Empty;
}
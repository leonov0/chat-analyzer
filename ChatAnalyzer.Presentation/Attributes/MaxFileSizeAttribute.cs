using System.ComponentModel.DataAnnotations;

namespace ChatAnalyzer.Presentation.Attributes;

public class MaxFileSizeAttribute(int maxFileSize) : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not IFormFile file) return ValidationResult.Success;

        return file.Length > maxFileSize ? new ValidationResult(ErrorMessageString) : ValidationResult.Success;
    }
}
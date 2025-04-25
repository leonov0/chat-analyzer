using ChatAnalyzer.Presentation.Attributes;

namespace ChatAnalyzer.Presentation.Requests;

public class CreateAnalysisDto
{
    [MaxFileSize(10485760, ErrorMessage = "The file size should not exceed 10 MB.")]
    public IFormFile File { get; set; }
}
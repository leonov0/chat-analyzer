using System.Net.Http.Json;
using ChatAnalyzer.Infrastructure.Options;
using ChatAnalyzer.Infrastructure.Responses;
using Microsoft.Extensions.Options;

namespace ChatAnalyzer.Infrastructure.Services;

public class SemanticKernelService(IOptions<GeminiOptions> options)
{
    private readonly HttpClient _httpClient = new()
    {
        BaseAddress = new Uri(options.Value.Endpoint)
    };

    public async Task<string?> GenerateReplyAsync(string prompt)
    {
        var body = new { contents = new[] { new { parts = new[] { new { text = prompt } } } } };
        var response = await _httpClient.PostAsJsonAsync($"?key={options.Value.ApiKey}", body);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<GeminiResponse>();
        return result?.Candidates.FirstOrDefault()?.Content.Parts.FirstOrDefault()?.Text;
    }
}
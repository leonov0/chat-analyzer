using ChatAnalyzer.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace ChatAnalyzer.Infrastructure.Services;

public class SemanticKernelService
{
    private readonly Kernel _kernel;

    public SemanticKernelService(IOptions<AzureOpenAIOptions> config)
    {
        var options = config.Value;

        _kernel = Kernel.CreateBuilder()
            .AddAzureOpenAIChatCompletion(
                options.DeploymentName,
                options.Endpoint,
                options.ApiKey)
            .Build();
    }

    public async Task<string?> AskAsync(string prompt)
    {
        var chat = _kernel.GetRequiredService<IChatCompletionService>();
        var result = await chat.GetChatMessageContentAsync(new ChatHistory(prompt));
        return result.Content;
    }
}
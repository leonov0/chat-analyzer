﻿namespace ChatAnalyzer.Infrastructure.Options;

public class AzureOpenAIOptions
{
    public string Endpoint { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public string DeploymentName { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string ApiVersion { get; set; } = string.Empty;
}
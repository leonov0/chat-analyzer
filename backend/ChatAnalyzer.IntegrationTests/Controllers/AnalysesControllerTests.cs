using System.Net;
using System.Net.Http.Headers;
using System.Text;
using FluentAssertions;

namespace ChatAnalyzer.IntegrationTests.Controllers;

public class AnalysesControllerTests(ChatAnalyzerWebApplicationFactory factory)
    : IClassFixture<ChatAnalyzerWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task CreateAnalysis_ValidChat_ReturnsSuccess()
    {
        // Arrange
        const string json = """
                            {
                              "name": "Test Chat",
                              "type": "personal_chat",
                              "id": 0,
                              "messages": [
                                {
                                  "id": 0,
                                  "type": "message",
                                  "date": "2025-05-07T20:18:04",
                                  "date_unixtime": "1746638284",
                                  "from": "0",
                                  "from_id": "0",
                                  "text": "0",
                                  "text_entities": [
                                    {
                                      "type": "plain",
                                      "text": "0"
                                    }
                                  ]
                                }
                              ]
                            }
                            """;
        var content = new MultipartFormDataContent();
        var fileContent = new ByteArrayContent(Encoding.UTF8.GetBytes(json));
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
        content.Add(fileContent, "file", "chat.json");

        // Act
        var response = await _client.PostAsync("/api/analyses", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var responseBody = await response.Content.ReadAsStringAsync();
        responseBody.Should().Contain("Test Chat");
    }
}
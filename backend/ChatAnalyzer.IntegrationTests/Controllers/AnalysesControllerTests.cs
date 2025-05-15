using System.Net;
using System.Net.Http.Json;
using System.Text;
using ChatAnalyzer.Presentation.Requests;
using FluentAssertions;
using static System.Net.Http.Headers.MediaTypeHeaderValue;

namespace ChatAnalyzer.IntegrationTests.Controllers;

public class AnalysesControllerTests(ChatAnalyzerWebApplicationFactory factory)
    : IClassFixture<ChatAnalyzerWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task CreateAnalysis_ValidChat_ReturnsSuccess()
    {
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
        fileContent.Headers.ContentType = Parse("application/json");
        content.Add(fileContent, "file", "chat.json");

        var response = await _client.PostAsync("/api/analyses", content);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var responseBody = await response.Content.ReadAsStringAsync();
        responseBody.Should().Contain("Test Chat");
    }

    [Fact]
    public async Task GetAll_ReturnsOkAndEmptyListInitially()
    {
        var response = await _client.GetAsync("/api/analyses");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("[");
    }

    [Fact]
    public async Task GetChat_NonExistentId_ReturnsNotFound()
    {
        var fakeId = Guid.NewGuid();

        var response = await _client.GetAsync($"/api/analyses/{fakeId}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddMessage_ToNonExistentAnalysis_ReturnsNotFound()
    {
        var fakeId = Guid.NewGuid();

        var request = new AddMessageDto
        {
            Message = "What is the summary?"
        };

        var response = await _client.PostAsJsonAsync($"/api/analyses/{fakeId}/messages", request);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateAnalysis_InvalidJson_ReturnsBadRequest()
    {
        const string badJson = """
                               { "invalid": true
                               """;

        var content = new MultipartFormDataContent();
        var fileContent = new ByteArrayContent(Encoding.UTF8.GetBytes(badJson));
        fileContent.Headers.ContentType = Parse("application/json");
        content.Add(fileContent, "file", "bad_chat.json");

        var response = await _client.PostAsync("/api/analyses", content);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var responseBody = await response.Content.ReadAsStringAsync();
        responseBody.Should().Contain("An error occurred while processing the file");
    }
}
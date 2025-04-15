using System.Text.Json;
using ChatAnalyzer.Application.Interfaces;
using ChatAnalyzer.Application.Services;
using ChatAnalyzer.Domain.Entities;
using ChatAnalyzer.Domain.Interfaces;
using Moq;

namespace ChatAnalyzer.UnitTests.Services;

public class AnalysisServiceTests
{
    private readonly Mock<IAnalysisRepository> _analysisRepositoryMock;
    private readonly AnalysisService _analysisService;
    private readonly Mock<IAnalyzer> _analyzerMock;
    private readonly Mock<ICryptoService> _cryptoServiceMock;
    private readonly Mock<IAnalysisMessageRepository> _messageRepositoryMock;

    public AnalysisServiceTests()
    {
        _analysisRepositoryMock = new Mock<IAnalysisRepository>();
        _analyzerMock = new Mock<IAnalyzer>();
        _cryptoServiceMock = new Mock<ICryptoService>();
        _messageRepositoryMock = new Mock<IAnalysisMessageRepository>();

        _analysisService = new AnalysisService(
            _analysisRepositoryMock.Object,
            _analyzerMock.Object,
            _cryptoServiceMock.Object,
            _messageRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateAsync_WhenValid_ShouldCreateAndReturnAnalysis()
    {
        var chat = new Chat
        {
            Name = "Chat 1",
            Messages =
            [
                new Message
                {
                    Id = 1,
                    Type = "message",
                    DateUnixtime = "1680000000",
                    From = "User A",
                    FromId = "user123",
                    TextEntities = [new TextEntity { Type = "plain", Text = "Hello" }]
                }
            ]
        };

        var userId = Guid.NewGuid();
        var analysisResult = "Analyzed content";
        var encryptedChat = "EncryptedJsonString";

        _analyzerMock.Setup(a => a.AnalyzeAsync(chat)).ReturnsAsync(analysisResult);
        _cryptoServiceMock.Setup(c => c.Encrypt(It.IsAny<string>())).Returns(encryptedChat);
        _analysisRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<Analysis>())).Returns(Task.CompletedTask);

        var result = await _analysisService.CreateAsync(chat, userId);

        Assert.Equal("Chat 1", result.Name);
        Assert.Equal(userId, result.UserId);
        Assert.Single(result.Messages);
        Assert.Equal(analysisResult, result.Messages[0].Content);
        Assert.Equal(encryptedChat, result.EncryptedChat);
    }

    [Fact]
    public async Task GetByIdAsync_WhenExists_ShouldReturnAnalysis()
    {
        var analysisId = Guid.NewGuid();
        var expectedAnalysis = new Analysis { Id = analysisId };

        _analysisRepositoryMock.Setup(r => r.GetByIdAsync(analysisId)).ReturnsAsync(expectedAnalysis);

        var result = await _analysisService.GetByIdAsync(analysisId);

        Assert.NotNull(result);
        Assert.Equal(analysisId, result.Id);
    }

    [Fact]
    public async Task GetAllAsync_WhenCalled_ShouldReturnUserAnalyses()
    {
        var userId = Guid.NewGuid();
        var analyses = new List<Analysis> { new() { UserId = userId } };

        _analysisRepositoryMock.Setup(r => r.GetAllAsync(userId)).ReturnsAsync(analyses);

        var result = (await _analysisService.GetAllAsync(userId)).ToArray();

        Assert.Single(result);
        Assert.Equal(userId, Assert.Single(result).UserId);
    }

    [Fact]
    public async Task AskAsync_WhenChatIsValid_ShouldAddMessages()
    {
        var analysisId = Guid.NewGuid();
        var chat = new Chat
        {
            Id = 1,
            Name = "Chat",
            Messages =
            [
                new Message
                {
                    Id = 1,
                    Type = "message",
                    DateUnixtime = "1680000000",
                    From = "User A",
                    FromId = "user123",
                    TextEntities = [new TextEntity { Type = "plain", Text = "Hi!" }]
                }
            ]
        };

        const string message = "How's it going?";
        const string reply = "All good!";
        const string encryptedChat = "EncryptedData";
        var serializedChat = JsonSerializer.Serialize(chat);

        var analysis = new Analysis
        {
            Id = analysisId,
            EncryptedChat = encryptedChat,
            Messages = []
        };

        _cryptoServiceMock.Setup(c => c.Decrypt(encryptedChat)).Returns(serializedChat);
        _analyzerMock.Setup(a => a.AskAsync(It.IsAny<Chat>(), message)).ReturnsAsync(reply);
        _messageRepositoryMock.Setup(m => m.CreateAsync(It.IsAny<AnalysisMessage>())).Returns(Task.CompletedTask);

        var result = await _analysisService.AskAsync(analysis, message);

        Assert.Equal(2, result.Messages.Count);
        Assert.Contains(result.Messages, m => m.Content == message);
        Assert.Contains(result.Messages, m => m.Content == reply);
    }

    [Fact]
    public async Task AskAsync_WhenChatCannotBeDeserialized_ShouldThrowException()
    {
        var analysis = new Analysis { EncryptedChat = "invalid" };
        _cryptoServiceMock.Setup(c => c.Decrypt(It.IsAny<string>())).Returns("not a valid chat json");

        await Assert.ThrowsAsync<JsonException>(() => _analysisService.AskAsync(analysis, "Hello?"));
    }
}
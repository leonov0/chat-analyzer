﻿using System.Text.Json.Serialization;

namespace ChatAnalyzer.Domain.Entities;

public class Chat
{
    [JsonPropertyName("id")] public long Id { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; } = string.Empty;
    [JsonPropertyName("type")] public string Type { get; set; } = string.Empty;
    [JsonPropertyName("messages")] public List<Message> Messages { get; set; } = [];
}

public class Message
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("type")] public string Type { get; set; } = string.Empty;
    [JsonPropertyName("date_unixtime")] public string DateUnixtime { get; set; } = string.Empty;
    [JsonPropertyName("from")] public string From { get; set; } = string.Empty;
    [JsonPropertyName("from_id")] public string FromId { get; set; } = string.Empty;
    [JsonPropertyName("text_entities")] public List<TextEntity> TextEntities { get; set; } = [];
}

public class TextEntity
{
    [JsonPropertyName("type")] public string Type { get; set; } = string.Empty;
    [JsonPropertyName("text")] public string Text { get; set; } = string.Empty;
}
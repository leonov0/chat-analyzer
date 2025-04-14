using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.Json;
using ChatAnalyzer.Application.Interfaces;
using ChatAnalyzer.Domain.Entities;
using ChatAnalyzer.Presentation.Requests;
using ChatAnalyzer.Presentation.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatAnalyzer.Presentation.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AnalysesController(IAnalysisService analysisService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var analyses = await analysisService.GetAllAsync(userId);

        return Ok(analyses.Select(a => new AnalysisPreviewDto
        {
            Id = a.Id,
            CreatedAt = a.CreatedAt,
            UpdatedAt = a.UpdatedAt
        }));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetChat(Guid id)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var analysis = await analysisService.GetByIdAsync(id);

        if (analysis == null) return NotFound();

        if (analysis.UserId != userId) return Forbid();

        return Ok(new AnalysisDto
        {
            Id = analysis.Id,
            Messages = analysis.Messages.Select(m => new AnalysisMessageDto
            {
                Id = m.Id,
                Content = m.Content,
                CreatedAt = m.CreatedAt,
                UpdatedAt = m.UpdatedAt
            }),
            CreatedAt = analysis.CreatedAt,
            UpdatedAt = analysis.UpdatedAt
        });
    }

    [HttpPost]
    public async Task<IActionResult> CreateAnalysis([FromForm] CreateAnalysisDto createAnalysisDto)
    {
        Chat? chat;

        try
        {
            await using var stream = createAnalysisDto.File.OpenReadStream();

            chat = await JsonSerializer.DeserializeAsync<Chat>(stream);
        }
        catch (Exception ex)
        {
            return BadRequest(new
                { Title = "An error occurred while processing the file.", Errors = new[] { ex.Message } });
        }

        if (chat == null) return BadRequest(new { Title = "Invalid JSON format." });

        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(chat);

        if (!Validator.TryValidateObject(chat, context, validationResults, true))
            return BadRequest(new
            {
                Title = "Validation failed.",
                Errors = validationResults.Select(vr => new { vr.ErrorMessage, vr.MemberNames })
            });

        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        try
        {
            var analysis = await analysisService.CreateAsync(chat, userId);

            return Created($"/api/analyses/{analysis.Id}", new AnalysisDto
            {
                Id = analysis.Id,
                Messages = analysis.Messages.Select(m => new AnalysisMessageDto
                {
                    Id = m.Id,
                    Content = m.Content,
                    CreatedAt = m.CreatedAt,
                    UpdatedAt = m.UpdatedAt
                }),
                CreatedAt = analysis.CreatedAt,
                UpdatedAt = analysis.UpdatedAt
            });
        }
        catch
        {
            return BadRequest(new { Title = "An error occurred while analyzing the chat. Please try again later." });
        }
    }
}
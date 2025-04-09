using ChatAnalyzer.Application.Interfaces;
using ChatAnalyzer.Application.Services;
using ChatAnalyzer.Domain.Interfaces;
using ChatAnalyzer.Infrastructure;
using ChatAnalyzer.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAnalysisService, AnalysisService>();
builder.Services.AddScoped<IAnalysisRepository, AnalysisRepository>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
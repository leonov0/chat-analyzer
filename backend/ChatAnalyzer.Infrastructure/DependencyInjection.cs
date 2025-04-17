using ChatAnalyzer.Application.Interfaces;
using ChatAnalyzer.Domain.Entities;
using ChatAnalyzer.Domain.Interfaces;
using ChatAnalyzer.Infrastructure.Options;
using ChatAnalyzer.Infrastructure.Persistence;
using ChatAnalyzer.Infrastructure.Repositories;
using ChatAnalyzer.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChatAnalyzer.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
            {
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.Configure<GeminiOptions>(configuration.GetSection(nameof(GeminiOptions)));
        services.Configure<EncryptionOptions>(configuration.GetSection(nameof(EncryptionOptions)));

        services.AddSingleton<SemanticKernelService>();
        services.AddSingleton<ICryptoService, CryptoService>();

        services.AddScoped<IAnalysisRepository, AnalysisRepository>();
        services.AddScoped<IAnalysisMessageRepository, AnalysisMessageRepository>();

        services.AddScoped<IAnalyzer, Analyzer>();

        return services;
    }
}
using ChatAnalyzer.Domain.Entities;
using ChatAnalyzer.Infrastructure.Persistence;
using ChatAnalyzer.IntegrationTests.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace ChatAnalyzer.IntegrationTests;

public class ChatAnalyzerWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            services.AddAuthentication("Test")
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", _ => { });

            services.PostConfigureAll<AuthenticationOptions>(options =>
            {
                options.DefaultAuthenticateScheme = "Test";
                options.DefaultChallengeScheme = "Test";
            });

            var sp = services.BuildServiceProvider();

            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var testUserId = Guid.Parse("11111111-1111-1111-1111-111111111111");

            if (db.Users.Any(u => u.Id == testUserId)) return;

            db.Users.Add(new ApplicationUser
            {
                Id = testUserId,
                Email = "testuser@example.com"
            });
            db.SaveChanges();
        });
    }
}
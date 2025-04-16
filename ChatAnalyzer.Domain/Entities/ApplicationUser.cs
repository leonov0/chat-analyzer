using Microsoft.AspNetCore.Identity;

namespace ChatAnalyzer.Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public List<Analysis> Analyses { get; set; } = [];
}
namespace ChatAnalyzer.Application.Interfaces;

public interface IAuthService
{
    Task RegisterAsync(string username, string email, string password);
    Task LoginAsync(string email, string password);
    Task LogoutAsync();
}
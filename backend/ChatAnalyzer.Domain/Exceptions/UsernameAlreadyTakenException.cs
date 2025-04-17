namespace ChatAnalyzer.Domain.Exceptions;

public class UsernameAlreadyTakenException(string username) : Exception($"Username '{username}' is already taken.");
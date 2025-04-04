namespace ChatAnalyzer.Domain.Exceptions;

public class EmailTakenException(string email) : Exception($"Email '{email}' is already taken.");
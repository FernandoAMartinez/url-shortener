namespace UrlShortener.Contracts;

public record RegisterRequest(long Id, string Username, string Email, string Password, DateTime Birthdate);
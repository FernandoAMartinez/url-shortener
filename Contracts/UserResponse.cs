namespace UrlShortener.Contracts;

public record UserResponse(long Id, string Username, string Email, DateTime CreatedAt, DateTime UpdatedAt);
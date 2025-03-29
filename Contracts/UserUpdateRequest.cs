namespace UrlShortener.Contracts;

public record UserUpdateRequest(long Id, string? Username, string? Email);
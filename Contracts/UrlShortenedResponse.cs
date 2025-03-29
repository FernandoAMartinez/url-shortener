namespace UrlShortener.Contracts;

public record UrlShortenedResponse(int Id,  string Url, string ShortCode, DateTime CreatedAt, DateTime UpdatedAt);
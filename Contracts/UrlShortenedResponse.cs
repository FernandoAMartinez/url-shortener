namespace UrlShortener.Contracts;

public record UrlShortenedResponse(long Id,  string Url, string ShortCode, DateTime CreatedAt, DateTime UpdatedAt, long AccessCount);
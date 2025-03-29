using UrlShortener.Contracts;

namespace UrlShortener.Repositories;

public interface IUrlRepository
{
    Task<UrlShortenedResponse> CreateShortenUrl(UrlShortenedRequest request);

    Task<UrlShortenedResponse> GetOriginalUrl(string shortenCode);
    string TestUrlShortener(string url);
}

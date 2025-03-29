using UrlShortener.Contracts;

namespace UrlShortener.Repositories;

public class UrlRepository : IUrlRepository
{
    public async Task<UrlShortenedResponse> CreateShortenUrl(UrlShortenedRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<UrlShortenedResponse> GetOriginalUrl(string shortenCode)
    {
        throw new NotImplementedException();
    }

    public string TestUrlShortener(string url) => MakeShortenCodeFromUrl(url);

    private string MakeShortenCodeFromUrl(string url)
    {
        char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-_".ToCharArray();
        var split = url.Split("/");
        var shortenCode = "";

        var random = new Random();

        foreach (var item in split.Skip(1))
        {
            foreach (var c in item)
            {
                var i = item.IndexOf(c) % alpha.Length;
                
                var j = random.Next(i, alpha.Length);
                //var i = random.Next(item.Length) % alpha.Length;

                //var character = alpha[i];
                var character = alpha[j];

                shortenCode += character;
            }
        }

        var half = shortenCode.Length / 2;

        shortenCode = shortenCode.Substring(half, half/2);

        return shortenCode;
    }
}

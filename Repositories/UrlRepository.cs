using Microsoft.IdentityModel.Abstractions;
using Supabase.Gotrue;
using Supabase.Realtime;
using Supabase.Storage;
using UrlShortener.Contracts;
using UrlShortener.Helpers;

namespace UrlShortener.Repositories;

public class UrlRepository : IUrlRepository
{
    private Supabase.Interfaces.ISupabaseClient<User, Session, RealtimeSocket, RealtimeChannel, Bucket, FileObject> _client;

    public UrlRepository(Supabase.Interfaces.ISupabaseClient<User, Session, RealtimeSocket, RealtimeChannel, Bucket, FileObject> client)
    {
        _client = client;
    }

    public async Task<UrlShortenedResponse> CreateShortenUrl(UrlShortenedRequest request)
    {
        var shortenCode = MakeShortenCodeFromUrl(request.Url);

        var model = new Models.Url
        {
            UrlAddress = request.Url,
            ShortCode = shortenCode,
            AccessCount = 0,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt= DateTime.UtcNow,
        };

        var response = await _client.From<Models.Url>()
            .Insert(model);

        if (response.Models.Count.Equals(0))
            return null;

        var url = response.Models.FirstOrDefault();

        return new UrlShortenedResponse(url.Id, url.UrlAddress, url.ShortCode, url.CreatedAt, url.UpdatedAt, url.AccessCount);
    }

    public async Task<UrlShortenedResponse> GetOriginalUrl(string shortenCode)
    {
        var response = await _client.From<Models.Url>().Where(x => x.ShortCode == shortenCode).Get();

        if (response.Models.Count.Equals(0))
            return null;

        var url = response.Models.FirstOrDefault();

        return new UrlShortenedResponse(url.Id, url.UrlAddress, url.ShortCode, url.CreatedAt, url.UpdatedAt, url.AccessCount);
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

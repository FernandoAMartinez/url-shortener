using Supabase.Gotrue;
using Supabase.Interfaces;
using Supabase.Realtime;
using Supabase.Storage;
using Supabase;
using UrlShortener.Extensions;

namespace UrlShortener.EndpointDefinitions;

public class SupabaseEndpointDefinition : IEndpointDefinition
{
    public void DefineServices(IServiceCollection services)
    {
        var url = Environment.GetEnvironmentVariable("SUPABASE_URL") ?? string.Empty;
        var key = Environment.GetEnvironmentVariable("SUPABASE_KEY") ?? string.Empty;

        services.AddScoped<ISupabaseClient<User, Session, RealtimeSocket, RealtimeChannel, Bucket, FileObject>, Supabase.Client>(_ =>
            new Supabase.Client(url, key, new SupabaseOptions()
            {
                AutoRefreshToken = true,
                AutoConnectRealtime = true
            })
        );
    }

    public void RegisterEndpoints(WebApplication app)
    {
        // Nothing
    }
}

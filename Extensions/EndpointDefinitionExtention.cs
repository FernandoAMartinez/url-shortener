namespace UrlShortener.Extensions;


public static class EndpointDefinitionExtensions
{
    public static void AddEnpointDefinitnions(
        this IServiceCollection services, params Type[] scanMarkers)
    {
        var endpointDefinitions = new List<IEndpointDefinition>();

        foreach (var marker in scanMarkers)
        {
            endpointDefinitions.AddRange(
                marker.Assembly.ExportedTypes
                .Where(x => typeof(IEndpointDefinition).IsAssignableFrom(x) && !x.IsAbstract && !x.IsInterface)
                .Select(Activator.CreateInstance).Cast<IEndpointDefinition>()
                );
        }

        foreach (var endpointDefinition in endpointDefinitions.Distinct().ToList())
        {
            endpointDefinition.DefineServices(services);
        }

        services.AddSingleton(endpointDefinitions as IReadOnlyCollection<IEndpointDefinition>);
    }

    public static void UseEndpointDefinitions(this WebApplication app)
    {
        var definitions = app.Services.GetRequiredService<IReadOnlyCollection<IEndpointDefinition>>();

        app.Use(async (context, next) =>
        {
            context.Response.OnStarting(() =>
            {
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                return Task.CompletedTask;
            });
            await next();
        });

        foreach (var endpointDefinition in definitions)
        {
            endpointDefinition.RegisterEndpoints(app);
        }
    }
}

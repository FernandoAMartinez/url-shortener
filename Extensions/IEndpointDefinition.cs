namespace UrlShortener.Extensions;

public interface IEndpointDefinition
{
    void RegisterEndpoints(WebApplication app);
    void DefineServices(IServiceCollection services);
}

using ExchangeAPI.Modules.Identity.Application;
using ExchangeAPI.Modules.Identity.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeAPI.Modules.Identity.WebAPI;

public static class ConfigureServices
{
    public static IServiceCollection AddIdentityModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentityApplicationServices();

        services.AddIdentityInfrastructureServices(configuration);

        return services;
    }

    public static IApplicationBuilder UseIdentityModule(this IApplicationBuilder app)
    {
        app.UseIdentityInfrastructure();

        return app;
    }
}
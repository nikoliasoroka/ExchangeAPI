using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using ExchangeAPI.Shared.Filters;

namespace ExchangeAPI;

public static class ConfigureServices
{
    public static void AddBootstrapperServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        services.AddControllers(options => options.Filters.Add<ApiExceptionFilterAttribute>())
            .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

        services.AddEndpointsApiExplorer();

        services.AddCors(options => options.AddPolicy("AllowAll", builder => builder
            .AllowAnyHeader()
            .AllowAnyMethod()
            .SetIsOriginAllowed(_ => true)
            .AllowCredentials()));

        services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
    }
}

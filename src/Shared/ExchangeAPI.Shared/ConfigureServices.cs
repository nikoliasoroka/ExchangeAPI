using ExchangeAPI.Shared.Settings;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace ExchangeAPI.Shared;

public static class ConfigureServices
{
    public static IServiceCollection AddShared(this IServiceCollection services, IConfiguration configuration)
    {
        ConfigurationHelper.Initialize(configuration);

        services.AddSwaggerGen(c =>
        {
            c.EnableAnnotations();
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "ExchangeAPI", Version = "v1" });
            c.CustomSchemaIds(type => type.ToString());

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please insert JWT with Bearer info field",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                        },
                        Array.Empty<string>()
                    }
            });
        });

        // configure Jwt Token Settings
        var jwtConfig = configuration.GetSection("Jwt");
        services.Configure<JwtSettings>(jwtConfig);

        ValidatorOptions.Global.LanguageManager.Enabled = false;

        return services;
    }

    public static IApplicationBuilder UseShared(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        return app;
    }
}
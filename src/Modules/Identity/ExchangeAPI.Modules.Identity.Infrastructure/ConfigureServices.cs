using ExchangeAPI.Modules.Identity.Application.Common.Interfaces.Services.Identity;
using ExchangeAPI.Modules.Identity.Infrastructure.Services.Identity;
using ExchangeAPI.Modules.Identity.Shared.Interfaces;
using ExchangeAPI.Shared.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;
using ExchangeAPI.Modules.Identity.Application.Common.Interfaces.Context;
using ExchangeAPI.Modules.Identity.Domain.Entities;
using ExchangeAPI.Modules.Identity.Infrastructure.Persistence.Context;
using ExchangeAPI.Modules.Identity.Infrastructure.Persistence.Extensions;
using ExchangeAPI.Modules.Identity.Application.Common.Interfaces.Services;
using ExchangeAPI.Modules.Identity.Infrastructure.Services;

namespace ExchangeAPI.Modules.Identity.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddIdentityInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextPool<DataContext>(options =>
        {
            options.UseSqlServer(ConfigurationHelper.SqlConnectionString);
        });

        services.AddScoped<IDataContext>(x => x.GetRequiredService<DataContext>());

        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddIdentity<AppUser, Role>().AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();

        services.Configure<DataProtectionTokenProviderOptions>(options => options.TokenLifespan = TimeSpan.FromMinutes(Convert.ToDouble(configuration["Jwt:ResetTokenLifespan"])));

        services.AddRepositories();

        services.AddJwtAuthentication(configuration);

        services.AddHttpClient<IOpenExchangeRatesService, OpenExchangeRatesService>();

        return services;
    }

    public static IApplicationBuilder UseIdentityInfrastructure(this IApplicationBuilder app)
    {
        app.MigrateDatabase();

        app.SeedStandardRoles();

        app.SeedActionTypes();

        app.SeedAccessRights();

        app.SeedAdminUser();

        return app;
    }

    #region Private

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services
            .AddSingleton<ICurrentUserService, CurrentUserService>()
            .AddTransient<IIdentityService, IdentityService>()
            .AddTransient<IUserService, UserService>()
            .AddScoped<IActivityService, ActivityService>()
            .AddScoped<ITokenService, TokenService>();
    }

    private static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwtBearerOptions =>
            {
                jwtBearerOptions.RequireHttpsMetadata = true;
                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

        return services;
    }

    #endregion
}
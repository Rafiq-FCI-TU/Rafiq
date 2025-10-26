using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Rafiq.Domain.Settings;
using System.Text;

namespace Rafiq.API.ServicesExtension;

public static class JwtServicesExtension
{
    public static IServiceCollection AddJwtSettings(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure JWT settings from appsettings.json
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        // Add JWT authentication
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(o =>
        {
            o.RequireHttpsMetadata = false;
            o.SaveToken = false;
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = configuration["JwtSettings:Issuer"],
                ValidAudience = configuration["JwtSettings:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"])),
                ClockSkew = TimeSpan.Zero
            };
        });

        return services;
    }
}

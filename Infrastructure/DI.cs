using System.Text;

using Application.SPI;

using Infrastructure.Authentication;
using Infrastructure.DB;
using Infrastructure.Services;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;

using WebAPI.Infrastructure.DB;
using Domain;

namespace Infrastructure;

public static class DI
{
    public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services, Microsoft.Extensions.Configuration.ConfigurationManager configuration)
    {
        // services.ConfigureAuthenticationServices(configuration);
        var jwtSettings = new JwtSettings();
        configuration.GetSection(JwtSettings.SectionName).Bind(jwtSettings);

        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        services.AddSingleton<IJWTTokenGenerator, JwtTokenGenerator>();

        // user ef core in memory db
        services.AddDbContext<DemoDBContext>(o => o.UseInMemoryDatabase("demo-db"));
        services.AddScoped<IDbContext>(provider => provider.GetRequiredService<DemoDBContext>());

        services.AddScoped<DBGenerator>();

        // apply identity
        services
            .AddDefaultIdentity<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<DemoDBContext>();

        services.AddIdentityServer()
            .AddApiAuthorization<ApplicationUser, DemoDBContext>();

        services.AddSingleton<IDateTimeService, DateTimeService>();
        services.AddTransient<IIdentityService, IdentityService>();

        services.AddScoped<IUserRepository, UserRepository>();
        // services.AddScoped<IAuthenticationService, AuthenticationService>();

        // Apply policy to role
        services.AddAuthorization(options =>
            options.AddPolicy("CanPurge", policy => policy.RequireRole(UserRoles.Admin)));

        services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer, // configuration[JwtSettings.SectionName + ":Issuer"],
                    ValidAudience = jwtSettings.Audience, // configuration[JwtSettings.SectionName + ":Audience"]
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
                };
            });

        // adding health check service.
        services.AddHealthChecks()
                .AddDbContextCheck<DemoDBContext>();

        return services;
    }
}



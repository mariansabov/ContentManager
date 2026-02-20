using ContentManager.Application.Common.Behavior;
using ContentManager.Application.Common.Interfaces;
using ContentManager.Domain.Entities;
using ContentManager.Infrastructure.Options;
using ContentManager.Infrastructure.Persistence;
using ContentManager.Infrastructure.Services;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ContentManager.Infrastructure.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigureServices(
            this IServiceCollection services,
            IConfiguration configuration,
            Assembly[] assemblies
        )
        {
            services
                .AddControllers()
                .AddJsonOptions(o =>
                {
                    o.JsonSerializerOptions.Converters.Add(
                        new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
                    );
                });

            services
                .AddJwtAuthentication(configuration)
                .AddHttpContextAccessor()
                .AddEndpointsApiExplorer()
                .AddSwaggerGen(c =>
                {
                    c.SwaggerDoc(
                        "v1",
                        new OpenApiInfo { Title = "ContentManager Api", Version = "v1" }
                    );
                })
                .AddDbContext(configuration)
                .AddMediatR(assemblies)
                .AddAutoMapper(cfg => cfg.AddMaps(assemblies))
                .AddValidatorsFromAssemblies(assemblies);

            return services;
        }

        public static IServiceCollection AddMediatR(
            this IServiceCollection services,
            Assembly[] assemblies
        )
        {
            return services
                .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies))
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        }

        public static IServiceCollection AddDbContext(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.Configure<AdminOptions>(configuration.GetSection("Admin"));

            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<IApplicationDatabaseContext>(provider =>
                provider.GetRequiredService<DatabaseContext>()
            );

            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtOptions>(configuration.GetSection("Jwt"));

            services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();

            services
                .AddAuthentication("Bearer")
                .AddJwtBearer(
                    "Bearer",
                    opt =>
                    {
                        var jwt = configuration.GetSection("Jwt").Get<JwtOptions>()!;
                        opt.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidIssuer = jwt.Issuer,

                            ValidateAudience = true,
                            ValidAudience = jwt.Audience,

                            ValidateLifetime = true,
                            
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(jwt.Key)
                            ),
                            NameClaimType = ClaimTypes.NameIdentifier,
                            RoleClaimType = ClaimTypes.Role,
                        };

                        opt.Events = new JwtBearerEvents
                        {
                            OnMessageReceived = ctx =>
                            {
                                if (!string.IsNullOrEmpty(ctx.Token))
                                {
                                    return Task.CompletedTask;
                                }

                                var fromCookie = ctx.HttpContext.Request.Cookies["access_token"];
                                if (!string.IsNullOrWhiteSpace(fromCookie))
                                {
                                    ctx.Token = fromCookie;
                                }

                                return Task.CompletedTask;
                            },
                        };
                    }
                );

            services.AddAuthorization();

            services.AddScoped<IJwtTokenService, JwtTokenService>();

            return services;
        }
    }
}

using ContentManager.Application.Common.Interfaces;
using ContentManager.Application.Features.Publications.Announcements;
using ContentManager.Infrastructure.Options;
using ContentManager.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using System.Reflection;
using ContentManager.Application.Common.Behavior;
using FluentValidation;

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
            services.AddControllers();

            services
                .AddHttpContextAccessor()
                .AddEndpointsApiExplorer()
                .AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ContentManager Api", Version = "v1" });
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
            services.Configure<AdminOptions>(
                configuration.GetSection("Admin"));

            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<IApplicationDatabaseContext>(provider =>
                provider.GetRequiredService<DatabaseContext>()
            );

            return services;
        }
    }
}

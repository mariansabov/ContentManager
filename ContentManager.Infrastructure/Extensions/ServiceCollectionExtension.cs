using ContentManager.Application.Common.Interfaces;
using ContentManager.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;

namespace ContentManager.Infrastructure.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigureServices(
            this IServiceCollection services,
            IConfiguration configuration
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
                .AddDbContext(configuration);

            return services;
        }

        public static IServiceCollection AddDbContext(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
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

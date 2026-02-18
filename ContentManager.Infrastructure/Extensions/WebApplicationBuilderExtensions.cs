using System.Reflection;
using Microsoft.AspNetCore.Builder;

namespace ContentManager.Infrastructure.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder, Assembly[] assemblies)
        {
            builder.Services.ConfigureServices(builder.Configuration, assemblies);

            Console.WriteLine("______________________________________________");

            return builder;
        }
    }
}

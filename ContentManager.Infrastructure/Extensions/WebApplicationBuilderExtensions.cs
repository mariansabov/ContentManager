using System.Reflection;
using Microsoft.AspNetCore.Builder;

namespace ContentManager.Infrastructure.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder, Assembly[] assemblies)
        {
            builder.Services.ConfigureServices(builder.Configuration, assemblies);

            var now = DateTime.Now;

            var hour = 7;

            var notNow = now.AddHours(hour);

            Console.WriteLine($"Current time: {now}; not now: {notNow}");

            Console.WriteLine("______________________________________________");

            return builder;
        }
    }
}

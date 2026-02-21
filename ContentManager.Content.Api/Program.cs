using ContentManager.Application.Features.Publications.Announcements;
using ContentManager.Infrastructure.Extensions;
using ContentManager.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ContentManager.Content.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var assemblies = new[] { typeof(Program).Assembly, typeof(CreateAnnouncementCommand).Assembly };

            var app = WebApplication.CreateBuilder(args).ConfigureServices(assemblies).Build().ConfigureApp();

            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

            context.Database.Migrate();
            //context.Seed();

            app.Run();
        }
    }
}
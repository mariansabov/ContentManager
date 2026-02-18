using ContentManager.Infrastructure.Extensions;
using ContentManager.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ContentManager.Admin.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var app = WebApplication.CreateBuilder(args).ConfigureServices().Build().ConfigureApp();

            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

            context.Database.Migrate();
            context.Seed();

            app.Run();
        }
    }
}

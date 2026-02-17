using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace ContentManager.Infrastructure.Extensions
{
    public static class WebApplicationExtensions
    {
        public static WebApplication ConfigureApp(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            return app;
        }
    }
}

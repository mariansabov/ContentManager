using ContentManager.Application.Common.Interfaces;
using ContentManager.Domain.Entities;
using ContentManager.Domain.Enums;
using ContentManager.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace ContentManager.Infrastructure.Persistence
{
    public class DatabaseContext(DbContextOptions<DatabaseContext> options)
        : DbContext(options), IApplicationDatabaseContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Publication> Publications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var configurationAssembly = typeof(UserConfiguration).Assembly;

            modelBuilder.ApplyConfigurationsFromAssembly(configurationAssembly);

            base.OnModelCreating(modelBuilder);
        }

        public void Seed()
        {
            SeedBootstrapAdmin();
        }

        private void SeedBootstrapAdmin()
        {
            if (Users.Any(u => u.Username == "admin"))
            {
                return;
            }

            var adminUser = new User
            {
                Username = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                Email = "admin@random.com",
                Role = UserRole.Admin,
                CreatedAt = DateTime.UtcNow
            };

            Users.Add(adminUser);

            SaveChanges();
        }
    }
}

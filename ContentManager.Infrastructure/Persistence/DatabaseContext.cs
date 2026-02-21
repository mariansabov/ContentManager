using ContentManager.Application.Common.Interfaces;
using ContentManager.Domain.Entities;
using ContentManager.Domain.Enums;
using ContentManager.Infrastructure.Configurations;
using ContentManager.Infrastructure.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ContentManager.Infrastructure.Persistence
{
    public class DatabaseContext(
        DbContextOptions<DatabaseContext> options,
        IOptions<AdminOptions> adminOptions)
            : DbContext(options), IApplicationDatabaseContext
    {
        private readonly AdminOptions _adminOptions = adminOptions.Value;

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
            if (Users.Any(u => u.Username == _adminOptions.Username))
            {
                return;
            }

            var adminUser = new User
            {
                Username = _adminOptions.Username,
                Email = _adminOptions.Email,
                Role = UserRole.Admin,
                CreatedAt = DateTime.UtcNow
            };

            adminUser.PasswordHash = new PasswordHasher<User>().HashPassword(adminUser, _adminOptions.Password);

            Users.Add(adminUser);
            
            SaveChanges();
        }
    }
}

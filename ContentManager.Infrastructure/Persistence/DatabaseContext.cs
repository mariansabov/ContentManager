using ContentManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using ContentManager.Application.Common.Interfaces;

namespace ContentManager.Infrastructure.Persistence
{
    public class DatabaseContext(DbContextOptions<DatabaseContext> options)
        : DbContext(options), IApplicationDatabaseContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Publication> Publications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}

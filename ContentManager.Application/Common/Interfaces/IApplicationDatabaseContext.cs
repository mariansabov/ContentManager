using ContentManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContentManager.Application.Common.Interfaces
{
    public interface IApplicationDatabaseContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Publication> Publications { get; set; }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}

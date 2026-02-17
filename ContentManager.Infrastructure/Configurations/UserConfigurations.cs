using ContentManager.Domain.Entities;
using ContentManager.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ContentManager.Infrastructure.Configurations
{
    public class UserConfigurations : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.HasIndex(u => u.Email).IsUnique();

            builder.HasIndex(u => u.Username).IsUnique();

            builder
                .Property(u => u.Role)
                .HasConversion(new EnumToStringConverter<UserRole>());
        }
    }
}

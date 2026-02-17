using ContentManager.Domain.Entities;
using ContentManager.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ContentManager.Infrastructure.Configurations
{
    public class PublicationConfigurations : IEntityTypeConfiguration<Publication>
    {
        public void Configure(EntityTypeBuilder<Publication> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder
                .Property(n => n.Status)
                .HasConversion(new EnumToStringConverter<PublicationStatus>());

            builder.HasOne(p => p.Author)
                .WithMany(u => u.Publications)
                .HasForeignKey(p => p.AuthorId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

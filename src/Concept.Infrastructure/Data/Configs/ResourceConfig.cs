using Concept.Core.Entities.Resource;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Concept.Infrastructure.Data.Configs
{
    public class ResourceConfig : IEntityTypeConfiguration<ResourceEntity>
    {
        public void Configure(EntityTypeBuilder<ResourceEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(50).IsRequired();
            builder.Property(x => x.ResourceType).HasMaxLength(50).IsRequired();
            builder.Property(x => x.ResourceKey).HasMaxLength(50).IsRequired();

            // 建立索引
            builder.HasIndex(x => x.ResourceType);
            builder.HasIndex(x => new { x.ResourceType, x.ResourceKey });

            // 設定關聯
            builder.HasOne(x => x.Owner)
                .WithMany()
                .HasForeignKey(x => x.OwnerId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.ConfigureAuditableEntity();
        }
    }
}

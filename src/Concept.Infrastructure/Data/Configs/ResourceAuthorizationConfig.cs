using Concept.Core.Entities.ResourceAuthorization;
using Concept.Core.Entities.User.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Concept.Infrastructure.Data.Configs
{
    public class ResourceAuthorizationConfig : IEntityTypeConfiguration<ResourceAuthorizationEntity>
    {
        public void Configure(EntityTypeBuilder<ResourceAuthorizationEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ResourceId).IsRequired();
            builder.Property(x => x.AuthorizedUserId).IsRequired();
            builder.Property(x => x.GrantedByUserId).IsRequired();
            builder.Property(x => x.ExpiresAt).HasColumnType("timestamp");
            builder.Property(x => x.PermissionLevel).HasConversion<string>().HasMaxLength(20).IsRequired();
            builder.Property(x => x.IsActive).IsRequired();

            // 設定關聯
            builder.HasOne(x => x.Resource)
                .WithMany()
                .HasForeignKey(x => x.ResourceId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.HasOne(x => x.AuthorizedUser)
                .WithMany()
                .HasForeignKey(x => x.AuthorizedUserId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.HasOne(x => x.GrantedByUser)
                .WithMany()
                .HasForeignKey(x => x.GrantedByUserId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            // 設定索引
            builder.HasIndex(x => new { x.ResourceId, x.AuthorizedUserId }).IsUnique();
            builder.HasIndex(x => new { x.AuthorizedUserId, x.IsActive });
            builder.HasIndex(x => x.GrantedByUserId);

            // 索引 - 過期時間
            // 用於查找和清理過期權限
            builder.HasIndex(x => x.ExpiresAt).HasFilter("(\"ExpiresAt\") IS NOT NULL");

            builder.ConfigureAuditableEntity();
        }
    }
}

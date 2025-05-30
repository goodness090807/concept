using Concept.Core.Entities.Role;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Concept.Infrastructure.Data.Configs
{
    public class RoleConfig : BaseEntityTypeConfiguration<RoleEntity>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<RoleEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(500);

            builder.HasIndex(x => x.Name).IsUnique();

            builder.HasData(
                new RoleEntity
                {
                    Id = 1,
                    Name = "Admin",
                    Description = "系統管理員"
                },
                new RoleEntity
                {
                    Id = 2,
                    Name = "StoreOwner",
                    Description = "商店擁有者"
                },
                new RoleEntity
                {
                    Id = 3,
                    Name = "StoreManager",
                    Description = "商店管理員"
                }
            );
        }
    }
}
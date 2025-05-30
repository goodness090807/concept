using Concept.Core.Entities.Role.Enums;
using Concept.Core.Entities.RolePermission;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Concept.Infrastructure.Data.Configs
{
    public class RolePermissionConfig : BaseEntityTypeConfiguration<RolePermissionEntity>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<RolePermissionEntity> builder)
        {
            builder.HasKey(x => new { x.RoleId, x.PermissionId });

            builder.HasOne(x => x.Role)
                .WithMany(x => x.RolePermissions)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Permission)
                .WithMany(x => x.RolePermissions)
                .HasForeignKey(x => x.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasData(
                new RolePermissionEntity
                {
                    RoleId = (int)Roles.StoreOwner, // StoreOwner
                    PermissionId = 1 // store:get
                },
                new RolePermissionEntity
                {
                    RoleId = (int)Roles.StoreOwner, // StoreOwner
                    PermissionId = 2 // store:grant-role
                },
                new RolePermissionEntity
                {
                    RoleId = (int)Roles.StoreManager, // StoreManager
                    PermissionId = 1 // store:get
                }
            );
        }
    }
}

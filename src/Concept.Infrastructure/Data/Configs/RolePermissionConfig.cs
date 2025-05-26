using Concept.Core.Entities.RolePermission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concept.Infrastructure.Data.Configs
{
    public class RolePermissionConfig : BaseEntityConfig<RolePermissionEntity>
    {
        public RolePermissionConfig() : base("RolePermissions")
        {
            HasKey(x => new { x.RoleId, x.PermissionId });
            Property(x => x.IsActive).IsRequired();
            Property(x => x.CreatedAt).IsRequired();
            Property(x => x.UpdatedAt).IsRequired();
            Property(x => x.CreatedBy).IsRequired();
            Property(x => x.UpdatedBy).IsRequired();
            HasRequired(x => x.Role)
                .WithMany(x => x.RolePermissions)
                .HasForeignKey(x => x.RoleId);
            HasRequired(x => x.Permission)
                .WithMany(x => x.RolePermissions)
                .HasForeignKey(x => x.PermissionId);
        }
    }
}

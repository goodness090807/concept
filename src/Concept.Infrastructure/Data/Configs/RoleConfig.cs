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
        }
    }
}
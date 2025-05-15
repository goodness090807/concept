using Concept.Core.Entities.User;
using Concept.Core.Entities.User.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Concept.Infrastructure.Data.Configs
{
    public class UserConfig : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.UserName).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Email).HasMaxLength(100).IsRequired();
            builder.Property(x => x.HashedPassword).HasMaxLength(100).IsRequired();
            builder.Property(x => x.UserStatus)
                   .HasConversion<string>()
                   .HasMaxLength(20)
                   .HasDefaultValue(UserStatus.Inactive)
                   .IsRequired();
            
            builder.ConfigureTimestampedEntity();
        }
    }
}

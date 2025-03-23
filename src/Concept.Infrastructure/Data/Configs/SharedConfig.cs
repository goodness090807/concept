using Concept.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Concept.Infrastructure.Data.Configs
{
    public static class SharedConfig
    {
        public static void ConfigureAuditableEntity<T>(this EntityTypeBuilder<T> builder) where T : AuditableEntity
        {
            builder.Property(x => x.CreatedAt).HasColumnType("timestamp").IsRequired();
            builder.Property(x => x.UpdatedAt).HasColumnType("timestamp").IsRequired();
        }
    }
}

using Concept.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Concept.Infrastructure.Data.Configs
{
    public static class SharedConfig
    {
        private const string TimestampWithTimeZone = "timestamptz";

        public static void ConfigureTimestampedEntity<T>(this EntityTypeBuilder<T> builder) where T : TimestampedEntity
        {
            builder.Property(x => x.CreatedAt).HasColumnType(TimestampWithTimeZone).IsRequired();
            builder.Property(x => x.UpdatedAt).HasColumnType(TimestampWithTimeZone).IsRequired();
        }
    }
}

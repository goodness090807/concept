using Concept.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Concept.Infrastructure.Data
{
    public abstract class BaseEntityTypeConfiguration<T> : IEntityTypeConfiguration<T> where T : TimestampedEntity
    {
        private const string TimestampWithTimeZone = "timestamptz";

        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(x => x.CreatedAt).HasColumnType(TimestampWithTimeZone).IsRequired();
            builder.Property(x => x.UpdatedAt).HasColumnType(TimestampWithTimeZone).IsRequired();

            ConfigureEntity(builder);
        }

        protected abstract void ConfigureEntity(EntityTypeBuilder<T> builder);
    }
}
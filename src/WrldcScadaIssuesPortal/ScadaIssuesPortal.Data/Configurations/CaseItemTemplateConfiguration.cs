using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScadaIssuesPortal.Core.Entities;
using System;

namespace ScadaIssuesPortal.Data.Configurations
{
    public class CaseItemTemplateConfiguration : IEntityTypeConfiguration<CaseItemTemplate>
    {
        public void Configure(EntityTypeBuilder<CaseItemTemplate> builder)
        {
            builder
            .HasIndex(b => b.Question)
            .IsUnique();

            builder
            .Property(b => b.ResponseType)
            .HasDefaultValue(ResponseType.ShortText);

            builder
            .Property(b => b.IsResponseRequired)
            .HasDefaultValue(true);

            builder
            .Property(b => b.SerialNum)
            .HasDefaultValue(1);

            // storing enum as string
            // https://docs.microsoft.com/en-us/ef/core/modeling/value-conversions
            builder
            .Property(e => e.ResponseType)
            .HasConversion(
                v => v.ToString(),
                v => (ResponseType)Enum.Parse(typeof(ResponseType), v));
        }
    }
}

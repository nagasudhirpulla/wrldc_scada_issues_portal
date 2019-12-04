using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScadaIssuesPortal.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScadaIssuesPortal.Data.Configurations
{
    public class ReportingCaseItemConfiguration : IEntityTypeConfiguration<ReportingCaseItem>
    {
        public void Configure(EntityTypeBuilder<ReportingCaseItem> builder)
        {
            builder
            .HasIndex(b => b.Question)
            .IsUnique();

            builder
            .Property(b => b.ResponseType)
            .HasDefaultValue(ResponseType.ShortText);

            builder
            .Property(b => b.SerialNum)
            .HasDefaultValue(1);

            // storing enum as string
            builder
            .Property(e => e.ResponseType)
            .HasConversion(
                v => v.ToString(),
                v => (ResponseType)Enum.Parse(typeof(ResponseType), v));
        }
    }
}

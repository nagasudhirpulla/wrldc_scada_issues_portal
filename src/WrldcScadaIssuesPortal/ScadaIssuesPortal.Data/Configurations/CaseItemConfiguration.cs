using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScadaIssuesPortal.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScadaIssuesPortal.Data.Configurations
{
    public class CaseItemConfiguration : IEntityTypeConfiguration<CaseItem>
    {
        public void Configure(EntityTypeBuilder<CaseItem> builder)
        {
            builder
            .HasIndex(b => b.Question)
            .IsUnique();

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
                v => (SurveyResponseType)Enum.Parse(typeof(SurveyResponseType), v));
        }
    }
}

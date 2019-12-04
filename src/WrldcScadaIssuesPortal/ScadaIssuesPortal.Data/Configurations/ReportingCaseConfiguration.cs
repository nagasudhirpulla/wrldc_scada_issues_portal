using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScadaIssuesPortal.Core.Entities;
using System;

namespace ScadaIssuesPortal.Data.Configurations
{
    public class ReportingCaseConfiguration : IEntityTypeConfiguration<ReportingCase>
    {
        public void Configure(EntityTypeBuilder<ReportingCase> builder)
        {
            // Default value of CreatedAt
            builder
                .Property(e => e.CreatedAt)
                .HasDefaultValue(DateTime.Now);

            // Default value of UpdatedAt and auto update modify
            builder
                .Property(e => e.UpdatedAt)
                .HasDefaultValue(DateTime.Now)
                .ValueGeneratedOnAddOrUpdate();
        }
    }
}

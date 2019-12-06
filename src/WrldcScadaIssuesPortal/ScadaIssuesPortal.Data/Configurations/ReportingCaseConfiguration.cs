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
                .HasDefaultValueSql("NOW()");

            // Default value of UpdatedAt and auto update modify
            builder
                .Property(e => e.UpdatedAt)
                .HasDefaultValueSql("NOW()")
                .ValueGeneratedOnAddOrUpdate();
        }
    }
}

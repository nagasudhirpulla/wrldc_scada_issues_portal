using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScadaIssuesPortal.Core.Entities;
using System;

namespace ScadaIssuesPortal.Data.Configurations
{
    public class ReportingCaseCommentConfiguration : IEntityTypeConfiguration<ReportingCaseComment>
    {
        public void Configure(EntityTypeBuilder<ReportingCaseComment> builder)
        {
            // storing enum as string
            builder
            .Property(e => e.Tag)
            .HasConversion(
                v => v.ToString(),
                v => (CommentTag)Enum.Parse(typeof(CommentTag), v));
        }
    }
}

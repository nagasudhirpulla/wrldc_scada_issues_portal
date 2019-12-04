using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScadaIssuesPortal.Core.Entities;

namespace ScadaIssuesPortal.Data.Configurations
{
    public class ReportingCaseConcernedConfiguration : IEntityTypeConfiguration<ReportingCaseConcerned>
    {
        // many to many relationship in ef-core - https://www.entityframeworktutorial.net/efcore/configure-many-to-many-relationship-in-ef-core.aspx
        public void Configure(EntityTypeBuilder<ReportingCaseConcerned> builder)
        {
            builder.HasKey(b => new { b.ReportingCaseId, b.UserId });

            builder
                .HasOne(rcc => rcc.ReportingCase)
                .WithMany(s => s.ConcernedAgencies)
                .HasForeignKey(rcc => rcc.ReportingCaseId);


            builder
                .HasOne(rcc => rcc.User)
                .WithMany()
                .HasForeignKey(rcc => rcc.UserId);
        }
    }
}

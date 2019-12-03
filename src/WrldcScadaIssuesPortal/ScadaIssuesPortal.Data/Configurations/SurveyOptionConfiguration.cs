using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScadaIssuesPortal.Core.Entities;

namespace ScadaIssuesPortal.Data.Configurations
{
    public class SurveyOptionConfiguration : IEntityTypeConfiguration<CaseItemOption>
    {
        public void Configure(EntityTypeBuilder<CaseItemOption> builder)
        {
            builder
            .HasIndex(b => new { b.OptionText, b.CaseItemId })
            .IsUnique();
        }
    }
}

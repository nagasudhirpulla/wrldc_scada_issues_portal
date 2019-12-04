using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScadaIssuesPortal.Core.Entities;

namespace ScadaIssuesPortal.Data.Configurations
{
    public class CaseItemOptionConfiguration : IEntityTypeConfiguration<CaseItemOption>
    {
        public void Configure(EntityTypeBuilder<CaseItemOption> builder)
        {
            builder
            .HasIndex(b => new { b.OptionText, b.CaseItemTemplateId })
            .IsUnique();

            builder
            .Property(b => b.SerialNum)
            .HasDefaultValue(1);
        }
    }
}

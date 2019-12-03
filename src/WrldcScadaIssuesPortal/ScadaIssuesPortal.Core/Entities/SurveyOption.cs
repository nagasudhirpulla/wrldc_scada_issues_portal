using ScadaIssuesPortal.Core.Interfaces;

namespace ScadaIssuesPortal.Core.Entities
{
    public class CaseItemOption : BaseEntity, IAggregateRoot
    {
        public string OptionText { get; set; }

        public CaseItem CaseItem { get; set; }
        public int CaseItemId { get; set; }
    }
}

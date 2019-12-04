using ScadaIssuesPortal.Core.Interfaces;

namespace ScadaIssuesPortal.Core.Entities
{
    public class CaseItemOption : BaseEntity, IAggregateRoot
    {
        public int SerialNum { get; set; }
        public string OptionText { get; set; }

        public CaseItem CaseItem { get; set; }
        public int CaseItemId { get; set; }
    }
}

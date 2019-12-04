using ScadaIssuesPortal.Core.Interfaces;

namespace ScadaIssuesPortal.Core.Entities
{
    public class CaseItemOption : BaseEntity, IAggregateRoot
    {
        public int SerialNum { get; set; }
        public string OptionText { get; set; }

        public CaseItemTemplate CaseItemTemplate { get; set; }
        public int CaseItemTemplateId { get; set; }
    }
}

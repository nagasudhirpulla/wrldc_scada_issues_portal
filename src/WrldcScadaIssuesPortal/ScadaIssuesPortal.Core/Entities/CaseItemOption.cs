using ScadaIssuesPortal.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ScadaIssuesPortal.Core.Entities
{
    public class CaseItemOption : BaseEntity, IAggregateRoot
    {
        public int SerialNum { get; set; }
        [Required]
        public string OptionText { get; set; }

        public CaseItemTemplate CaseItemTemplate { get; set; }
        public int CaseItemTemplateId { get; set; }
    }
}

using ScadaIssuesPortal.Core.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ScadaIssuesPortal.Core.Entities
{
    public class CaseItemTemplate : BaseEntity, IAggregateRoot
    {
        public int SerialNum { get; set; } = 1;
        [Required]
        public string Question { get; set; }
        [Required]
        public ResponseType ResponseType { get; set; } = ResponseType.ShortText;
        public ICollection<CaseItemOption> Options { get; set; } = new List<CaseItemOption>();
        public bool IsResponseRequired { get; set; }
    }
}

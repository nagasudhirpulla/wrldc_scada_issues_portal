using ScadaIssuesPortal.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ScadaIssuesPortal.Core.Entities
{
    public class ReportingCaseItem : BaseEntity, IAggregateRoot
    {
        public ReportingCase Case { get; set; }
        public int CaseId { get; set; }

        public int SerialNum { get; set; }
        [Required]
        public string Question { get; set; }
        public string Response { get; set; }
        [Required]
        public ResponseType ResponseType { get; set; } = ResponseType.ShortText;
    }
}

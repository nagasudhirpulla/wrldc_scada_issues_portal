using ScadaIssuesPortal.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ScadaIssuesPortal.Core.Entities
{
    public class ReportingCaseComment : AuditableEntity, IAggregateRoot
    {
        public ReportingCase ReportingCase { get; set; }
        public int ReportingCaseId { get; set; }

        [Required]
        public string Comment { get; set; }
        [Required]
        public CommentTag Tag { get; set; }
    }
}

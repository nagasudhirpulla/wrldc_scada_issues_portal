using Microsoft.AspNetCore.Identity;
using ScadaIssuesPortal.Core.Interfaces;

namespace ScadaIssuesPortal.Core.Entities
{
    public class ReportingCaseConcerned : BaseEntity, IAggregateRoot
    {
        public int ReportingCaseId { get; set; }
        public ReportingCase ReportingCase { get; set; }

        public string UserId { get; set; }
        public IdentityUser User { get; set; }
    }
}

using ScadaIssuesPortal.Core.Interfaces;
using System.Collections.Generic;
using System.Text;

namespace ScadaIssuesPortal.Core.Entities
{
    public class CaseItem : BaseEntity, IAggregateRoot
    {
        public ReportingCase Case { get; set; }
        public int CaseId { get; set; }

        public int SerialNum { get; set; }
        public string Question { get; set; }
        public string Response { get; set; }
        public SurveyResponseType ResponseType { get; set; }
        public ICollection<CaseItemOption> Options { get; set; } = new List<CaseItemOption>();
        public bool IsResponseRequired { get; set; }
    }
}

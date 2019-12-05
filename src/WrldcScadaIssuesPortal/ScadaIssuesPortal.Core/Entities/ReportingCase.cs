using ScadaIssuesPortal.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace ScadaIssuesPortal.Core.Entities
{
    public class ReportingCase : BaseEntity, IAggregateRoot
    {
        public List<ReportingCaseItem> CaseItems { get; set; } = new List<ReportingCaseItem>();

        public List<ReportingCaseConcerned> ConcernedAgencies { get; set; } = new List<ReportingCaseConcerned>();

        public DateTime DownTime { get; set; }
        public DateTime ResolutionTime { get; set; }
        public string ResolutionRemarks { get; set; }
        public string AdminRemarks { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

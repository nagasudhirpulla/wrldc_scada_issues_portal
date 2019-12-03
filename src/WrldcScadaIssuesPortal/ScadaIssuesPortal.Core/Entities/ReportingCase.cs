using ScadaIssuesPortal.Core.Interfaces;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace ScadaIssuesPortal.Core.Entities
{
    public class ReportingCase : BaseEntity, IAggregateRoot
    {
        public ICollection<CaseItem> SurveyItems { get; set; } = new List<CaseItem>();

        public IdentityUser ConcernedAgency { get; set; }
        public string ConcernedAgencyId { get; set; }

        public DateTime ResolutionTime { get; set; }
        public string ResolutionRemarks { get; set; }
        public string AdminRemarks { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

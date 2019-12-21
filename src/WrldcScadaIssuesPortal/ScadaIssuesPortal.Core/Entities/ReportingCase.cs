using Microsoft.AspNetCore.Identity;
using ScadaIssuesPortal.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ScadaIssuesPortal.Core.Entities
{
    public class ReportingCase : BaseEntity, IAggregateRoot
    {
        // todo use it as AuditableEntity
        public List<ReportingCaseItem> CaseItems { get; set; } = new List<ReportingCaseItem>();

        public List<ReportingCaseConcerned> ConcernedAgencies { get; set; } = new List<ReportingCaseConcerned>();

        public List<ReportingCaseComment> Comments { get; set; } = new List<ReportingCaseComment>();

        public DateTime DownTime { get; set; }
        public DateTime ResolutionTime { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public IdentityUser CreatedBy { get; set; }
        [Required]
        public string CreatedById { get; set; }
    }
}

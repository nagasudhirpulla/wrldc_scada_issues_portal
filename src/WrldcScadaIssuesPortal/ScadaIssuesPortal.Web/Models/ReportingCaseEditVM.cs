using ScadaIssuesPortal.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ScadaIssuesPortal.Web.Models
{
    public class ReportingCaseEditVM
    {
        public List<ReportingCaseItem> CaseItems { get; set; } = new List<ReportingCaseItem>();
        public List<ReportingCaseConcerned> ConcernedAgencies { get; set; } = new List<ReportingCaseConcerned>();
        [Display(Name = "New Agency")]
        public string ConcernedAgencyId { get; set; }
        public DateTime DownTime { get; set; }
        public DateTime ResolutionTime { get; set; }
        public string ResolutionRemarks { get; set; }
        public string AdminRemarks { get; set; }
    }
}

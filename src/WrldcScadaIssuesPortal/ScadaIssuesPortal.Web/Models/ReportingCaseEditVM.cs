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
        [Display(Name = "Concerned People")]
        public List<ReportingCaseConcerned> ConcernedAgencies { get; set; } = new List<ReportingCaseConcerned>();
        [Display(Name = "New Agency")]
        public string ConcernedAgencyId { get; set; }
        [Display(Name = "Issue Time")]
        public DateTime DownTime { get; set; }
        public DateTime ResolutionTime { get; set; }
        public string ResolutionRemarks { get; set; }
        public string AdminRemarks { get; set; }
    }
}

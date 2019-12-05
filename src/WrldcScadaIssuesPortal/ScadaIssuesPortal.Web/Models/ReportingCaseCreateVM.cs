using ScadaIssuesPortal.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ScadaIssuesPortal.Web.Models
{
    public class ReportingCaseCreateVM
    {
        public List<CaseItemTemplate> CaseItems { get; set; } = new List<CaseItemTemplate>();
        public List<string> Responses { get; set; } = new List<string>();
        public List<string> ChoiceTexts { get; set; } = new List<string>();
        public string ConcernedAgencyId { get; set; }
        public DateTime DownTime { get; set; }
        public DateTime ResolutionTime { get; set; }
        public string ResolutionRemarks { get; set; }
        public string AdminRemarks { get; set; }
    }
}

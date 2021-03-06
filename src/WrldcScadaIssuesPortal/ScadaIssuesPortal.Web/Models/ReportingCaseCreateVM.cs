﻿using Microsoft.AspNetCore.Http;
using ScadaIssuesPortal.App.Extensions;
using ScadaIssuesPortal.Core;
using ScadaIssuesPortal.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ScadaIssuesPortal.Web.Models
{
    // https://stackoverflow.com/questions/56588900/how-to-validate-uploaded-file-in-asp-net-core
    public class ReportingCaseCreateVM
    {
        public List<CaseItemTemplate> CaseItems { get; set; } = new List<CaseItemTemplate>();
        public List<string> Responses { get; set; } = new List<string>();
        public List<string> ChoiceTexts { get; set; } = new List<string>();
        [Display(Name = "Concerned People")]
        public string ConcernedAgencyId { get; set; }
        [Display(Name = "Issue Time")]
        public DateTime DownTime { get; set; }
        public DateTime ResolutionTime { get; set; }
        public string ResolutionRemarks { get; set; }
        public string AdminRemarks { get; set; }
        //[FileExtensions(Extensions = ".jpg,.jpeg,.jpe,.jif,.jfif,.jfi,.png,.gif,.webp,.tiff,.tif,.bmp,.svg,.svgz,.pdf,.doc,.docx,.rar,.zip,.xls,.xlsx,.csv,.ppt,.pptx", ErrorMessage = "Incorrect file format")]
        [MaxFileSize]
        [AllowedExtensions]
        public IFormFile Attachment { get; set; }
    }
}

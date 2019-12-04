using System.ComponentModel.DataAnnotations;

namespace ScadaIssuesPortal.Web.Models
{
    public class CaseItemTemplateOptionAddVM
    {
        [Required]
        public string NewOption { get; set; }
        [Required]
        public int CaseItemTemplateId { get; set; }
    }
}

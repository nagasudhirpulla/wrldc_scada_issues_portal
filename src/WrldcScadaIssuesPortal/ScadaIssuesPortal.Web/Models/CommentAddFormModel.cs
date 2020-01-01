using ScadaIssuesPortal.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace ScadaIssuesPortal.Web.Models
{
    public class CommentAddFormModel
    {
        [Required]
        public string Comment { get; set; }
        [Required]
        public CommentTag Tag { get; set; }
    }
}

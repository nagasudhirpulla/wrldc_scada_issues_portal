using Microsoft.AspNetCore.Http;
using ScadaIssuesPortal.App.Extensions;

namespace ScadaIssuesPortal.Web.Models
{
    public class CaseAttachmentFormModel
    {
        [MaxFileSize]
        [AllowedExtensions]
        public IFormFile CaseAttachment { get; set; }
        public int Id { get; set; }
    }
}

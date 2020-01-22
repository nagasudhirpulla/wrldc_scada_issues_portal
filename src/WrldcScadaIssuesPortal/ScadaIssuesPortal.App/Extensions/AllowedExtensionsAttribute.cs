using Microsoft.AspNetCore.Http;
using ScadaIssuesPortal.Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace ScadaIssuesPortal.App.Extensions
{
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _Extensions;
        public AllowedExtensionsAttribute()
        {
            _Extensions = FileAttachmentConstants.AllowedExtensions;
        }

        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("Unable to retrieve file");
            }
            var file = value as IFormFile;
            var extension = Path.GetExtension(file.FileName);
            if (!(file == null))
            {
                if (!_Extensions.Contains(extension.ToLower()))
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return $"This extension is not allowed";
        }
    }
}

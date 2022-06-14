using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace HpLayer.Attributes {
    public class AllowedExtensions : ValidationAttribute {
        private readonly string[] _extensions;
        private readonly string _errorMessage;

        public AllowedExtensions (string[] extensions) {
            _extensions = extensions;
            var removeDots = extensions.Select (x => x.Replace ('.', ' ')).ToArray ();
            var extString = string.Join (" - ", removeDots);
            _errorMessage = $"پسوند های  {extString} مورد تایید می باشد.";
        }

        protected override ValidationResult IsValid (
            object value, ValidationContext validationContext) {
            var file = value as IFormFile;
            if (file != null) {
                var extension = Path.GetExtension (file.FileName);
                if (!_extensions.Contains (extension.ToLower ())) {
                    return new ValidationResult (_errorMessage);
                }
            }
            return ValidationResult.Success;
        }
    }
}
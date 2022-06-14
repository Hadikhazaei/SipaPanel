using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

// layer
using HpLayer.Extensions;

namespace HpLayer.Attributes {
    public class AttrRequestSizeLimit : ValidationAttribute {
        private readonly long _maxFileSize;
        private readonly string _errorMessage;
        public AttrRequestSizeLimit (long maxFileSize) {
            _maxFileSize = maxFileSize;
            _errorMessage = $"حداکثر حجم فایل {maxFileSize.SizeSuffix()} می باشد";
        }

        protected override ValidationResult IsValid (
            object value, ValidationContext validationContext) {
            var file = value as IFormFile;
            if (file != null) {
                if (file.Length > _maxFileSize) {
                    return new ValidationResult (_errorMessage);
                }
            }
            return ValidationResult.Success;
        }
    }
}
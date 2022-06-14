using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace HpLayer.Attributes {
    public class PersianLettersRegular : RegularExpressionAttribute, IClientModelValidator {
        private const string _pattern = @"^[\u0600-\u06FF,\u0590-\u05FF\s]*$";
        public PersianLettersRegular (string pattern = _pattern) : base (pattern) {
            ErrorMessage = "لطفا از حروف فارسی استفاده نمائید";
        }
        public void AddValidation (ClientModelValidationContext context) {
            context.Attributes.Add ("style", "direction: rtl");
            context.Attributes.Add ("data-val-regex", ErrorMessage);
            context.Attributes.Add ("data-val-regex-pattern", _pattern);
        }
    }
}
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace HpLayer.Attributes {
    public class NumericOnlyRegular : RegularExpressionAttribute, IClientModelValidator {
        private const string _pattern = @"^[0-9]*$";
        public NumericOnlyRegular (string pattern = _pattern) : base (pattern) {
            ErrorMessage = "فقط عدد معتبر است";
        }
        public void AddValidation (ClientModelValidationContext context) {
            context.Attributes.Add ("style", "direction: ltr");
            context.Attributes.Add ("data-val-regex", ErrorMessage);
            context.Attributes.Add ("data-val-regex-pattern", _pattern);
        }
    }
}
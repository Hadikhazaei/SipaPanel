using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace HpLayer.Attributes {
    public class CellNumberRegular : RegularExpressionAttribute, IClientModelValidator {
        private const string _pattern = @"^(\+98|0)?9\d{9}$";
        public CellNumberRegular (string pattern = _pattern) : base (pattern) {
            ErrorMessage = "شماره همراه وارد شده معتبر نیست";
        }
        public void AddValidation (ClientModelValidationContext context) {
            context.Attributes.Add ("style", "direction: ltr");
            context.Attributes.Add ("data-val", "true");
            context.Attributes.Add ("data-val-regex", ErrorMessage);
            context.Attributes.Add ("data-val-regex-pattern", _pattern);
        }
    }
}
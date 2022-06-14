using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace HpLayer.Attributes {
    public class PhoneNumberRegular : RegularExpressionAttribute, IClientModelValidator {
        private const string _pattern = @"^(\+98|0)\d{10}$";
        public PhoneNumberRegular (string pattern = _pattern) : base (pattern) {
            ErrorMessage = "شماره تلفن وارد شده معتبر نیست";
        }
        public void AddValidation (ClientModelValidationContext context) {
            context.Attributes.Add ("style", "direction: ltr");
            context.Attributes.Add ("data-val-regex", ErrorMessage);
            context.Attributes.Add ("data-val-regex-pattern", _pattern);
        }
    }
}
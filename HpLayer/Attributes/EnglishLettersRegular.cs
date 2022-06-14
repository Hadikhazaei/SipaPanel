using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace HpLayer.Attributes {
    public class EnglishLettersRegular : RegularExpressionAttribute, IClientModelValidator {
        private const string _pattern = "^[a-zA-Z_]*$";
        public EnglishLettersRegular (string pattern = _pattern) : base (pattern) {
            ErrorMessage = "لطفا از حروف انگلیسی استفاده نمائید";
        }
        public void AddValidation (ClientModelValidationContext context) {
            context.Attributes.Add ("style", "direction: ltr");
            context.Attributes.Add ("data-val-regex", ErrorMessage);
            context.Attributes.Add ("data-val-regex-pattern", _pattern);
        }
    }
}
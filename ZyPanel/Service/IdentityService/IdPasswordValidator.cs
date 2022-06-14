using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

// 
using DbLayer.DbTable.Identity;

namespace ZyPanel.Service.IdentityService {
    public class IdPasswordValidator : PasswordValidator<AppUser> {
        public IdPasswordValidator (IdentityErrorDescriber errors) : base (errors) { }

        public override async Task<IdentityResult> ValidateAsync (UserManager<AppUser> manager, AppUser user, string password) {
            var errors = new List<IdentityError> ();
            if (string.IsNullOrWhiteSpace (password)) {
                errors.Add (new IdentityError {
                    Code = "PasswordIsNotSet",
                        Description = "لطفا کلمه‌ی عبور را تکمیل کنید."
                });
                return IdentityResult.Failed (errors.ToArray ());
            }

            if (string.IsNullOrWhiteSpace (user?.UserName)) {
                errors.Add (new IdentityError {
                    Code = "UserNameIsNotSet",
                        Description = "لطفا نام کاربری را تکمیل کنید."
                });
                return IdentityResult.Failed (errors.ToArray ());
            }

            // First use the built-in validator
            var result = await base.ValidateAsync (manager, user, password);
            errors = result.Succeeded ? new List<IdentityError> () : result.Errors.ToList ();

            // Extending the built-in validator
            if (password.ToLower ().Contains (user.UserName.ToLower ())) {
                errors.Add (new IdentityError {
                    Code = "PasswordContainsUserName",
                        Description = "کلمه‌ی عبور نمی‌تواند حاوی قسمتی از نام کاربری باشد."
                });
                return IdentityResult.Failed (errors.ToArray ());
            }

            if (!IsSafePasword (password)) {
                errors.Add (new IdentityError {
                    Code = "PasswordIsNotSafe",
                        Description = "کلمه‌ی عبور وارد شده به سادگی قابل حدس زدن است."
                });
                return IdentityResult.Failed (errors.ToArray ());
            }
            return !errors.Any () ? IdentityResult.Success : IdentityResult.Failed (errors.ToArray ());
        }

        private static bool AreAllCharsEuqal (string data) {
            if (string.IsNullOrWhiteSpace (data)) {
                return false;
            }

            data = data.ToLowerInvariant ();
            var firstElement = data.ElementAt (0);
            var euqalCharsLen = data.ToCharArray ().Count (x => x == firstElement);
            if (euqalCharsLen == data.Length) {
                return true;
            }

            return false;
        }

        private bool IsSafePasword (string password) => (string.IsNullOrWhiteSpace (password) || password.Length < 5 || AreAllCharsEuqal (password)) ? false : true;
    }
}
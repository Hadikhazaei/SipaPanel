using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

// 
using DbLayer.DbTable.Identity;
using ZyPanel.Helper.Utils;

namespace ZyPanel.Service.IdentityService {
    public class IdUserValidator : UserValidator<AppUser> {
        public IdUserValidator (IdentityErrorDescriber errors) : base (errors) { }

        public override async Task<IdentityResult> ValidateAsync (UserManager<AppUser> manager, AppUser user) {
            // First use the built-in validator
            var result = await base.ValidateAsync (manager, user);
            var errors = result.Succeeded ? new List<IdentityError> () : result.Errors.ToList ();
            // Extending the built-in validator
            ValidateEmail (user, errors);
            ValidateUserName (user, errors);
            return !errors.Any () ? IdentityResult.Success : IdentityResult.Failed (errors.ToArray ());
        }

        private void ValidateEmail (AppUser user, List<IdentityError> errors) {
            var userEmail = user?.Email;
            if (string.IsNullOrWhiteSpace (userEmail)) {
                if (string.IsNullOrWhiteSpace (userEmail)) {
                    errors.Add (new IdentityError {
                        Code = "EmailIsNotSet",
                            Description = "لطفا اطلاعات ایمیل را تکمیل کنید."
                    });
                }
            }
        }

        private static void ValidateUserName (AppUser user, List<IdentityError> errors) {
            var userName = user?.UserName;
            if (string.IsNullOrWhiteSpace (userName)) {
                if (string.IsNullOrWhiteSpace (userName)) {
                    errors.Add (new IdentityError {
                        Code = "UserIsNotSet",
                            Description = "لطفا اطلاعات کاربری را تکمیل کنید."
                    });
                }
                return;
            }
            if (userName.IsNumeric () || userName.ContainsNumber ()) {
                errors.Add (new IdentityError {
                    Code = "BadUserNameError",
                        Description = "نام کاربری وارد شده نمی ‌تواند حاوی اعداد باشد."
                });
            }
            if (userName.HasConsecutiveChars ()) {
                errors.Add (new IdentityError {
                    Code = "BadUserNameError",
                        Description = "نام کاربری وارد شده معتبر نیست."
                });
            }
        }
    }
}
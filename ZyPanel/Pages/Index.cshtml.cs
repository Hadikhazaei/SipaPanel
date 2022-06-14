using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

// 
using DbLayer.DbTable.Identity;
using DbLayer.Enums;
using ZyPanel.Service.IdentityService;

namespace ZyPanel.Pages {

    [AllowAnonymous]
    public class IndexModel : PageModel {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        public IndexModel (SignInManager<AppUser> signInManager, UserManager<AppUser> userManager) {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult OnGet () {
            // if (_signInManager.IsSignedIn (User)) {
            //     return RedirectToPage ("Index", new { area = "Reports" });
            // }
            return Page ();
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel {
            [Required]
            [Display (Name = "نام کاربری")]
            public string UserName { get; set; }

            [Required]
            [Display (Name = "کلمه عبور")]
            [DataType (DataType.Password)]
            public string Password { get; set; }

            [Display (Name = "Remember me")]
            public bool RememberMe { get; set; }
        }

        public async Task<IActionResult> OnPostAsync (string returnUrl = null) {
            returnUrl = returnUrl ?? Url.Content ("~/");

            if (ModelState.IsValid) {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager
                    .PasswordSignInAsync (Input.UserName, Input.Password, Input.RememberMe,
                        lockoutOnFailure : false);

                if (result.Succeeded) {
                    string redirectUrl = "";
                    var user = await _userManager.FindByNameAsync (Input.UserName);
                    var roles = await _userManager.GetRolesAsync (user);
                    switch (roles[0]) {
                        case nameof (RoleType.CoRole):
                            {
                                redirectUrl = "/Reports";
                                break;
                            }
                        case nameof (RoleType.PlanningRole):
                            {
                                redirectUrl = "/Planning";
                                break;
                            }
                        case nameof (RoleType.ProductionManagerRole):
                            {
                                redirectUrl = "/Production";
                                break;
                            }
                        case nameof (RoleType.ProductionClerkRole):
                            {
                                redirectUrl = "/production/todo";
                                break;
                            }
                        default:
                            break;
                    }
                    return LocalRedirect (redirectUrl);
                }
                // if (result.RequiresTwoFactor) {
                //     return RedirectToPage ("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                // }
                // if (result.IsLockedOut) {
                //     // _logger.LogWarning ("User account locked out.");
                //     return RedirectToPage ("./Lockout");
                // } else {
                //     ModelState.AddModelError (string.Empty, "Invalid login attempt.");
                //     return Page ();
                // }
            }

            // If we got this far, something failed, redisplay form
            return Page ();
        }

        public async Task<IActionResult> OnGetLogOutAsync () {
            await _signInManager.SignOutAsync ();
            return RedirectToAction ("/");
        }
    }
}
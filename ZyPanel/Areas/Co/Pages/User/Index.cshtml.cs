using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

//
using DbLayer.Context;
using DbLayer.DbTable.Identity;
using DbLayer.Enums;
using HpLayer.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;
using ZyPanel.Helper.Utils;
using ZyPanel.Helper.Vmodel;

namespace ZyPanel.Areas.Co.Pages.User {
    public class IndexModel : PageModel {
        public readonly int _pageSize = 12;
        public readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public IndexModel (AppDbContext context, UserManager<AppUser> userManager) {
            _context = context;
            _userManager = userManager;
        }

        [TempData]
        public string Alert { get; set; }

        public class InputModel {
            [Display (Name = "نام و نام خانوادگی : ")]
            public string FullName { get; set; }

            [EnglishLettersRegular]
            [Display (Name = "نام کاربری : ")]
            [Required (ErrorMessage = ConstValues.RqError)]
            public string UserName { get; set; }

            [DataType (DataType.Password)]
            [Display (Name = "کلمه عبور")]
            [Required (ErrorMessage = "اجباری می باشد")]
            [StringLength (100, ErrorMessage = "حداقل {2} حرف میتواند می باشد.", MinimumLength = 5)]
            public string Password { get; set; }

            [DataType (DataType.Password)]
            [Display (Name = "تکرار کلمه عبور")]
            [Compare ("Password", ErrorMessage = "کلمه عبور و تکرار آن تطابق ندارند.")]
            public string ConfirmPassword { get; set; }

            [Display (Name = "سالن : ")]
            public HallType HallType { get; set; } = HallType.Default;

            [Display (Name = "خط : ")]
            public long? TblHallId { get; set; }

            [Display (Name = "نقش : ")]
            public RoleType UserRoleType { get; set; } = RoleType.CoRole;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class ListModel {
            public string Id { get; set; }

            public string UserName { get; set; }

            public string DisplayName { get; set; }

            public string HallTitle { get; set; }

            public List<string> Roels { get; set; }
        }

        public PaginatedList<ListModel> List { get; set; }

        public async Task OnGetAsync ([FromQuery] FilterQs vm) {
            var users = _userManager.Users.AsNoTracking ();
            List = await PaginatedList<ListModel>.CreateAsync (
                users.Include (x => x.TblHall)
                .Select (x => new ListModel {
                    Id = x.Id,
                        UserName = x.UserName,
                        DisplayName = x.DisplayName,
                        HallTitle = x.TblHall.FullTitle,
                }), vm.P, _pageSize
            );
        }

        public async Task<IActionResult> OnPostAsync () {
            if (ModelState.IsValid) {
                try {
                    bool puser = false;
                    if ((Input.UserRoleType == RoleType.ProductionClerkRole)) {
                        if (!Input.TblHallId.HasValue) {
                            throw new Exception ($"برای کاربر {Input.UserName} خط انتخاب نشده است.");
                        }
                        puser = true;
                    }
                    var user = new AppUser {
                        Email = Input.UserName,
                        UserName = Input.UserName,
                        FullName = Input.FullName,
                    };
                    if (puser) {
                        user.TblHallId = Input.TblHallId;
                    }
                    var result = await _userManager.CreateAsync (user, Input.Password);
                    if (result.Succeeded) {
                        var roleName = Input.UserRoleType.ToString ();
                        await _userManager.AddToRoleAsync (user, roleName);
                        Alert = ModelStateType.A200.ModelStateAsText ();
                    } else {
                        foreach (var error in result.Errors) {
                            ModelState.AddModelError (string.Empty, error.Description);
                        }
                        Alert = ModelState.ModelStateAsError ();
                    }
                } catch (Exception ex) {
                    ModelState.AddModelError ("", ex.Message);
                    Alert = ModelState.ModelStateAsError ();
                }
            }
            return RedirectToPage ("./Index");
        }

        // handler
        public PartialViewResult OnGetCreate () {
            return Partial ("_Create", new InputModel ());
        }

        public async Task<IActionResult> OnGetLineAsSelectAsync (byte hallType) {
            try {
                var halls = await _context.TblHall
                    .Where (x => (byte) x.HallType == hallType)
                    .Select (x => new SelectListItem {
                        Value = x.Id.ToString (),
                            Text = x.FullTitle
                    }).ToListAsync ();
                return new OkObjectResult (halls);
            } catch {
                return BadRequest (ConstValues.ErrRequest);
            }
        }
    }
}
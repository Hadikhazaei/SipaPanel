using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// 
using DbLayer.Context;
using DbLayer.DbTable;
using DbLayer.Enums;
using ZyPanel.Areas.Shared;
using ZyPanel.Helper.Utils;
using ZyPanel.Helper.Vmodel;

namespace ZyPanel.Areas.BaseInfo.Pages.Defect {
    public class IndexModel : FetchRootPage<TblDefect> {
        public IndexModel (AppDbContext context) : base (context) { }

        public class InputModel {
            [Display (Name = "عنوان : ")]
            [Required (ErrorMessage = ConstValues.RqError)]
            public string Title { get; set; }

            [Display (Name = "نوع ضایعات : ")]
            [Required (ErrorMessage = ConstValues.RqError)]
            public DefectType DefectType { get; set; }

            [Display (Name = "نوع قطعه : ")]
            [Required (ErrorMessage = ConstValues.RqError)]
            public DefectLineType DefectLineType { get; set; } = DefectLineType.NonBlok;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class ListModel {
            public long Id { get; set; }

            public string Title { get; set; }

            public string DefectTitle { get; set; }

            public string DefectTargetTitle { get; set; }
        }

        public PaginatedList<ListModel> List { get; set; }

        public async Task OnGetAsync ([FromQuery] FilterQs vm) {
            var entity = _dbSet.AsNoTracking ();
            if (!string.IsNullOrEmpty (vm.Filter)) {
                entity = _dbSet.Where (x => x.Title.Contains (vm.Filter));
            }
            List = await PaginatedList<ListModel>.CreateAsync (
                entity.Select (x => new ListModel {
                    Id = x.Id, Title = x.Title,
                        DefectTitle = x.DefectTitle,
                        DefectTargetTitle = x.DefectLineTitle
                }), vm.P, _pageSize
            );
        }

        public async Task<IActionResult> OnPostAsync () {
            if (ModelState.IsValid) {
                try {
                    if (EditKey != null) {
                        await EditItemExtend (x => x.Title,
                            x => x.DefectType, x => x.DefectLineType);
                        return RedirectToPage ("./Index");
                    } else {
                        await AddItemExtend<InputModel> (Input);
                    }
                } catch (Exception ex) {
                    ModelState.AddModelError ("", ex.Message);
                    Alert = ModelState.ModelStateAsError ();
                }
            }
            return RedirectToPage ("./Index");
        }

        public PartialViewResult OnGetCreate () => base.HandlerCreatePartial<InputModel> ();

        public async Task<PartialViewResult> OnGetEditAsync (long id) {
            EditKey = id.ToString ();
            var result = await FindAsync (id);
            return Partial ("_Create", new InputModel {
                Title = result.Title,
                    DefectType = result.DefectType,
                    DefectLineType = result.DefectLineType
            });
        }
    }
}
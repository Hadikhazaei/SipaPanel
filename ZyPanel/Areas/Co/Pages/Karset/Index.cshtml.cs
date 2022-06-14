using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// 
using CldLayer.Persian;
using DbLayer.Context;
using DbLayer.DbTable;
using DbLayer.Enums;
using ZyPanel.Areas.Shared;
using ZyPanel.Helper.Utils;
using ZyPanel.Helper.Vmodel;

namespace ZyPanel.Areas.Co.Pages.Karset {
    public class IndexModel : FetchRootPage<TblKarset> {
        public IndexModel (AppDbContext context) : base (context) { }

        public class InputModel {
            [Display (Name = "نوع ذوب : ")]
            [Required (ErrorMessage = ConstValues.RqError)]
            public KarsetType KarsetType { get; set; }

            [Display (Name = "تناژ قطعه (kg) : ")]
            public int ProductTonnage { get; set; }

            [Display (Name = "تناژ برنامه (kg) : ")]
            public int PlanningTonnage { get; set; }

            [Display (Name = "تاریخ : ")]
            public string PersianCreatDate { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class ListModel {
            public long Id { get; set; }

            public string KarsetTitle { get; set; }

            public double ProductTonnageAsTone { get; set; }

            public double PlanningTonnageAsTone { get; set; }

            public string PersianCreatDate { get; set; }

            public double Available { get; set; }
        }

        public PaginatedList<ListModel> List { get; set; }

        public async Task OnGetAsync ([FromQuery] FilterQs vm) {
            List = await PaginatedList<ListModel>.CreateAsync (
                _dbSet.OrderByDescending (x => x.Id)
                .Select (x => new ListModel {
                    Id = x.Id,
                        KarsetTitle = x.KarsetTitle,
                        PersianCreatDate = x.PersianCreatDate,
                        ProductTonnageAsTone = x.ProductTonnageAsTone,
                        PlanningTonnageAsTone = x.PlanningTonnageAsTone,
                        Available = x.Available
                }), vm.P, _pageSize
            );
        }

        public async Task<IActionResult> OnPostAsync () {
            if (ModelState.IsValid) {
                try {
                    if (string.IsNullOrEmpty (Input.PersianCreatDate)) {
                        Input.PersianCreatDate = DateTime.Now.ToShortPersianDateTimeString ();
                    }
                    await AddItem (new TblKarset {
                        KarsetType = Input.KarsetType,
                            ProductTonnage = Input.ProductTonnage,
                            PlanningTonnage = Input.PlanningTonnage,
                            PersianCreatDate = Input.PersianCreatDate
                    });
                    Alert = ModelStateType.A200.ModelStateAsText ();
                } catch (Exception ex) {
                    ModelState.AddModelError ("", ex.Message);
                    Alert = ModelState.ModelStateAsError ();
                }
            }
            return RedirectToPage ("./Index");
        }

        // handler
        public PartialViewResult OnGetCreate () => base.HandlerCreatePartial<InputModel> ();
    }
}
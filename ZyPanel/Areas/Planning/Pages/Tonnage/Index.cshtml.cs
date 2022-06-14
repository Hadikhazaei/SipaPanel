using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// 
using CldLayer.Persian;
using DbLayer.Context;
using DbLayer.DbTable;
using ZyPanel.Areas.Shared;
using ZyPanel.Helper.Utils;

namespace ZyPanel.Areas.Planning.Pages.Tonnage {

    public class IndexModel : FetchRootPage<TblTonnage> {
        public IndexModel (AppDbContext context) : base (context) { }

        public class InputModel {

            [Display (Name = "تاریخ : ")]
            public string PersianTonnageDate { get; set; }

            [Display (Name = "تناژ مصرفی (kg) : ")]
            public int UsedTonnageAsKg { get; set; }

            public long TblPlanningId { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [BindProperty (SupportsGet = true)]
        public long Id { get; set; }

        public string PlanningInfo { get; set; }

        public class ListModel {
            public long Id { get; set; }
            public long UsedTonnageAsKg { get; set; }

            public string UsedTonnageAsTone { get; set; }

            public string PersianTonnageDate { get; set; }
        }

        public List<ListModel> List { get; set; }

        public async Task OnGetAsync () {
            List = await _dbSet
                .Where (x => x.TblPlanningId == Id)
                .OrderByDescending (x => x.TonnageDate)
                .Select (x => new ListModel {
                    Id = x.Id,
                        UsedTonnageAsKg = x.UsedTonnageAsKg,
                        UsedTonnageAsTone = x.UsedTonnageAsTone,
                        PersianTonnageDate = x.PersianTonnageDate
                }).ToListAsync ();
        }

        public async Task<IActionResult> OnPostAsync () {
            if (ModelState.IsValid) {
                try {
                    var planning = await _context.TblPlanning.SingleAsync (x => x.Id == Id);
                    var tonnageDate = Input.PersianTonnageDate.ToGregorianDateTimeOrDefault ();
                    if (tonnageDate.Date < planning.BeginDate.Date || tonnageDate.Date > planning.FinishDate.Date) {
                        throw new Exception ($"تاریخ می بایست در بازه {planning.PersianBeginDate} تا {planning.PFinishDate} باشد.");
                    }
                    await AddItem (new TblTonnage {
                        TblPlanningId = Id,
                            UsedTonnageAsKg = Input.UsedTonnageAsKg,
                            PersianTonnageDate = Input.PersianTonnageDate,
                    });
                    Alert = ModelStateType.A200.ModelStateAsText ();
                } catch (Exception ex) {
                    ModelState.AddModelError ("", ex.Message);
                    Alert = ModelState.ModelStateAsError ();
                }
            }
            return RedirectToPage ("./Index");
        }

        public PartialViewResult OnGetCreate () => base.HandlerCreatePartial<InputModel> ();
    }

}
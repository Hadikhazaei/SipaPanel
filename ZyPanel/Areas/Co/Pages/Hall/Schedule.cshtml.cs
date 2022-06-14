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
using ZyPanel.Helper.Vmodel;

namespace ZyPanel.Areas.Co.Pages.Hall {
    public class ScheduleModel : FetchRootPage<TblHallSchedule> {
        public ScheduleModel (AppDbContext context) : base (context, "_CreateSchedule") { }

        [BindProperty (SupportsGet = true)]
        public long Id { get; set; }

        public string HallInfo { get; set; }

        public class InputModel {
            [Display (Name = "سال : ")]
            [Required (ErrorMessage = ConstValues.RqError)]
            [Range (1, int.MaxValue, ErrorMessage = ConstValues.RgError)]
            public int Year { get; set; }

            [Display (Name = "زمان : ")]
            public TimeSpan Interval { get; set; }

            public long TblHallId { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class ListModel {
            public long Id { get; set; }
            public int Year { get; set; }

            public TimeSpan Interval { get; set; }
        }

        public List<ListModel> List { get; set; }

        public async Task OnGetAsync ([FromQuery] FilterQs vm) {
            var hallInfo = await _context.TblHall.FirstOrDefaultAsync (x => x.Id == Id);
            HallInfo = $@"سالن : {hallInfo.FullTitle}";
            List = await _dbSet.Where (x => x.TblHallId == Id)
                .OrderByDescending (x => x.Year)
                .Select (x => new ListModel {
                    Id = x.Id,
                        Year = x.Year,
                        Interval = x.Interval
                }).ToListAsync ();
        }

        public async Task<IActionResult> OnPostAsync () {
            try {
                if (Input.Year < PersianCulture.GetPersianYear (DateTime.Now)) {
                    throw new Exception ($"سال '{Input.Year}' معتبر نمی باشد.");
                }
                if (await _dbSet.AnyAsync (x => x.TblHallId == Id && x.Year == Input.Year)) {
                    throw new Exception ($"سال '{Input.Year}' موجود می باشد.");
                }
                Input.TblHallId = Id;
                await AddItemExtend<InputModel> (Input);
                Alert = ModelStateType.A200.ModelStateAsText ();
            } catch (Exception ex) {
                ModelState.AddModelError ("", ex.Message);
                Alert = ModelState.ModelStateAsError ();
            }
            return RedirectToPage ("./Schedule");
        }

        public PartialViewResult OnGetCreate () => base.HandlerCreatePartial<InputModel> ();
    }
}
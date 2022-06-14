using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// 
using DbLayer.Context;
using DbLayer.DbTable;
using ZyPanel.Areas.Shared;
using ZyPanel.Helper.Utils;

namespace ZyPanel.Areas.Co.Pages.Schedule {
    public class IndexModel : FetchRootPage<TblSchedule> {
        public IndexModel (AppDbContext context) : base (context) { }

        public class InputModel {
            [Range (0, 23, ErrorMessage = "بازه مجاز از {1} تا {2} می باشد")]
            [Display (Name = "ساعت (24h) : ")]
            public byte BeginHour { get; set; }

            [Range (0, 59, ErrorMessage = "بازه مجاز از {1} تا {2} می باشد")]
            [Display (Name = "دقیقه : ")]
            public byte BeginMinute { get; set; }

            public TimeSpan BeginTime => TimeSpan.FromHours (BeginHour) + TimeSpan.FromMinutes (BeginMinute);

            [Range (0, 23, ErrorMessage = "بازه مجاز از {1} تا {2} می باشد")]
            [Display (Name = "ساعت (24h) : ")]
            public byte FinishHour { get; set; }

            [Range (0, 59, ErrorMessage = "بازه مجاز از {1} تا {2} می باشد")]
            [Display (Name = "دقیقه : ")]
            public byte FinishMinute { get; set; }

            public TimeSpan FinishTime => TimeSpan.FromHours (FinishHour) + TimeSpan.FromMinutes (FinishMinute);

            public long Id { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class ListModel {
            public long Id { get; set; }

            public List<Times> Times { get; set; }

            public string ScheduleTitle { get; set; }
        }

        public List<ListModel> List { get; set; }

        public async Task OnGetAsync () {
            var list = new List<ListModel> ();
            var schedule = await _dbSet.ToListAsync ();
            var parents = schedule.Where (x => !x.SubsetId.HasValue);
            foreach (var item in parents) {
                var scheduleItem = new ListModel {
                    Id = item.Id,
                    ScheduleTitle = item.ScheduleTitle
                };
                scheduleItem.Times = schedule
                    .Where (x => x.SubsetId == item.Id)
                    .Select (x => new Times {
                        Id = x.Id,
                            BeginTime = x.BeginTime?.ToString (@"hh\:mm"),
                            FinishTime = x.FinishTime?.ToString (@"hh\:mm"),
                    }).ToList ();
                list.Add (scheduleItem);
            }
            List = list;
        }

        public async Task<IActionResult> OnPostAsync () {
            if (ModelState.IsValid) {
                try {
                    var item = await _dbSet.FindAsync (Input.Id);
                    item.BeginTime = Input.BeginTime;
                    item.FinishTime = Input.FinishTime;
                    await EditItem (item);
                    Alert = ModelStateType.A200.ModelStateAsText ();
                } catch (Exception ex) {
                    ModelState.AddModelError ("", ex.Message);
                    Alert = ModelState.ModelStateAsError ();
                }
            }
            return RedirectToPage ("./Index");
        }

        // handler
        public async Task<PartialViewResult> OnGetCreate (long id) {
            byte defaultValue = 0;
            var result = await _dbSet.FirstOrDefaultAsync (x => x.Id == id);
            return Partial ("_Create", new InputModel {
                Id = id,
                    // begin
                    BeginHour = result.BeginTime.HasValue ? (byte) result.BeginTime.Value.Hours : defaultValue,
                    BeginMinute = result.BeginTime.HasValue ? (byte) result.BeginTime.Value.Minutes : defaultValue,
                    // finish
                    FinishHour = result.FinishTime.HasValue ? (byte) result.FinishTime.Value.Hours : defaultValue,
                    FinishMinute = result.FinishTime.HasValue ? (byte) result.FinishTime.Value.Minutes : defaultValue,
            });
        }

        public class Times {
            public long Id { get; set; }

            public string BeginTime { get; set; }

            public string FinishTime { get; set; }
        }
    }
}
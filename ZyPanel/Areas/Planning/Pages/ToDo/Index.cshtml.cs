using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

// 
using CldLayer.Persian;
using DbLayer.Context;
using DbLayer.DbTable;
using DbLayer.Enums;
using ZyPanel.Areas.Shared;
using ZyPanel.Helper.Utils;
using ZyPanel.Service.IdentityService;

namespace ZyPanel.Areas.Planning.Pages.ToDo {
    public class IndexModel : FetchRootPage<TblPlanning> {
        public IndexModel (AppDbContext context) : base (context) { }

        public class InputModel {
            [StringLength (50)]
            [Display (Name = "عنوان برنامه : ")]
            [Required (ErrorMessage = ConstValues.RqError)]
            public string Title { get; set; }

            public DateTime BeginDate { get; set; }

            public DateTime FinishDate { get; set; }

            public List<SelectListItem> Halls { get; set; }

            [Display (Name = "انتخاب خط : ")]
            [Required (ErrorMessage = ConstValues.RqError)]
            [Range (1, long.MaxValue, ErrorMessage = ConstValues.RgError)]
            public long TblHallId { get; set; }

            [Display (Name = "تعداد شیفت : ")]
            [Required (ErrorMessage = ConstValues.RqError)]
            [Range (1, short.MaxValue, ErrorMessage = ConstValues.RgError)]
            public short SchdeduleCount { get; set; } = 1;

            public ScheduleType ScheduleType { get; set; } = ScheduleType.Couple;

            // helper
            public string UserId { set; get; }

            [Display (Name = "تاریخ شروع : ")]
            [Required (ErrorMessage = ConstValues.RqError)]
            public string PersianBeginDate { get; set; }

            [Display (Name = "تاریخ پایان : ")]
            [Required (ErrorMessage = ConstValues.RqError)]
            public string PersianFinishDate { get; set; }

            public bool CanEdit { get; set; } = true;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class ListModel {
            public long Id { get; set; }
            public string PlanningTitle { get; set; }
            public string BeginDate { get; set; }
            public string FinishDate { get; set; }
            public string HallTitle { get; set; }
            // 
            public string ScheduleTitle { get; set; }
            public short SchdeduleCount { get; set; }
            // 
            public string UserName { get; set; }
            public string CreatedDate { get; set; }
            public short ConsideredDays { get; set; }
            // 
            public bool IsDoing { get; set; }
        }

        public PaginatedList<ListModel> List { get; set; }

        public async Task OnGetAsync (int p = 1) {
            var entity = _dbSet.Include (x => x.TblHall)
                .Include (x => x.AppUser).AsNoTracking ();
            List = await PaginatedList<ListModel>.CreateAsync (
                entity.OrderByDescending (x => x.Id)
                .Select (x => new ListModel {
                    Id = x.Id,
                        PlanningTitle = x.Title,
                        BeginDate = x.PersianBeginDate,
                        FinishDate = x.PFinishDate,
                        HallTitle = x.TblHall.FullTitle,
                        ScheduleTitle = x.ScheduleTitle,
                        SchdeduleCount = x.SchdeduleCount,
                        UserName = x.AppUser.UserName,
                        CreatedDate = x.PersianCreatedDate,
                        ConsideredDays = x.ConsideredDays,
                        IsDoing = DateTime.Now.Date >= x.BeginDate.Date && DateTime.Now.Date <= x.FinishDate.Date
                }), p, _pageSize
            );
        }

        public async Task<IActionResult> OnPostAsync () {
            if (ModelState.IsValid) {
                try {
                    Input.UserId = HttpContext.User.Identity.GetUserId ();
                    Input.BeginDate = Input.PersianBeginDate.ToGregorianDateTimeOrDefault ();
                    Input.FinishDate = Input.PersianFinishDate.ToGregorianDateTimeOrDefault ();
                    if (Input.BeginDate > Input.FinishDate) {
                        throw new Exception ("تاریخ شروع از پایان نمی تواند بزرگتر باشد.");
                    }
                    // create
                    if (EditKey == null) {
                        if (Input.BeginDate.Date < DateTime.Now.Date) {
                            throw new Exception ($"تاریخ <bdo class='ltr'>'{Input.PersianBeginDate}'</bdo> نامعتبر می باشد.");
                        }
                        if (await _dbSet.AnyAsync (x => Input.BeginDate >= x.BeginDate && Input.BeginDate <= x.FinishDate && x.TblHallId == Input.TblHallId)) {
                            var hall = await _context.TblHall.FirstOrDefaultAsync (x => x.Id == Input.TblHallId);
                            throw new Exception ($"در بازه تاریخی <bdo class='ltr'>{Input.PersianFinishDate} - {Input.PersianBeginDate}</bdo> برای خط '{hall.Line}' برنامه تعریف شده است.");
                        }
                        await AddItemExtend<InputModel> (Input);
                    } else {
                        var editKey = long.Parse (EditKey);
                        var item = await FindAsync (editKey);
                        if (Input.BeginDate.Date != item.BeginDate.Date || Input.FinishDate.Date != item.FinishDate.Date) {
                            if (Input.BeginDate.Date != item.BeginDate.Date && Input.BeginDate.Date < DateTime.Now.Date) {
                                throw new Exception ($"تاریخ <bdo class='ltr'>'{Input.PersianBeginDate}'</bdo> نامعتبر می باشد.");
                            }
                            if (await _dbSet.AnyAsync (x => Input.BeginDate >= x.BeginDate && Input.BeginDate <= x.FinishDate &&
                                    x.TblHallId == Input.TblHallId && x.Id != item.Id)) {
                                var hall = await _context.TblHall.FirstOrDefaultAsync (x => x.Id == Input.TblHallId);
                                throw new Exception ($"در بازه تاریخی <bdo class='ltr'>{Input.PersianFinishDate} - {Input.PersianBeginDate}</bdo> برای خط '{hall.Line}' برنامه تعریف شده است.");
                            }
                            item.BeginDate = Input.BeginDate;
                            item.FinishDate = Input.FinishDate;
                        }
                        if (await TryUpdateModelAsync<TblPlanning> (item, "",
                                x => x.Title, x => x.TblHallId, x => x.SchdeduleCount, x => x.ScheduleType, x => x.UserId
                            )) {
                            await _context.SaveChangesAsync ();
                        }
                    }
                    Alert = ModelStateType.A200.ModelStateAsText ();
                } catch (Exception ex) {
                    ModelState.AddModelError ("", ex.Message);
                    Alert = ModelState.ModelStateAsError ();
                }
            }
            return RedirectToPage ("./Index");
        }

        // handler
        public async Task<PartialViewResult> OnGetCreate () {
            var model = new InputModel ();
            model.Halls = await _FetchLinesAsSelectAsync ();
            return Partial ("_Create", model);
        }

        public async Task<PartialViewResult> OnGetEditAsync (long id) {
            if (await _context.TblProductionInfo.AnyAsync (x => x.TblPlanningId == id)) {
                return Partial ("_Create", new InputModel {
                    CanEdit = false
                });
            }
            EditKey = id.ToString ();
            var item = await FindAsNotTrackedAsync (id);
            var selectedList = await _FetchLinesAsSelectAsync ();
            return Partial ("_Create", new InputModel {
                ScheduleType = item.ScheduleType,
                    Title = item.Title,
                    Halls = selectedList,
                    TblHallId = item.TblHallId,
                    SchdeduleCount = item.SchdeduleCount,
                    PersianBeginDate = item.BeginDate.ToShortPersianDateString ().Replace ("/", "-"),
                    PersianFinishDate = item.FinishDate.ToShortPersianDateString ().Replace ("/", "-"),
                    CanEdit = true
            });
        }

        private async Task<List<SelectListItem>> _FetchLinesAsSelectAsync () {
            var result = await _context
                .TblHall.Select (x => new {
                    Value = x.Id.ToString (),
                        Text = x.FullTitle,
                        _HelperSort = x.HallType
                }).OrderBy (x => x._HelperSort).ToListAsync ();
            var selectedList = result.Select (x => new SelectListItem { Value = x.Value, Text = x.Text }).ToList ();
            selectedList.Insert (0, new SelectListItem () { Value = "0", Text = "انتخاب کنید" });
            return selectedList;
        }
    }
}
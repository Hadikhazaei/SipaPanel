using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

// 
using CldLayer.Persian;
using DbLayer.Context;
using DbLayer.DbTable;
using DbLayer.DbTable.Identity;
using DbLayer.Enums;
using HpLayer.Extensions;
using ZyPanel.Areas.Shared;
using ZyPanel.Helper.Utils;
using ZyPanel.Service.IdentityService;

namespace ZyPanel.Areas.Production.Pages.ToStop {
    public class IndexModel : ModifyRootPage<TblStopInfo> {
        private readonly UserManager<AppUser> _userManager;

        private string RangeError = "مقدار وارد شده می بایست در بازه 0 الی 5 باشد.";

        public IndexModel (AppDbContext context, UserManager<AppUser> userManager) : base (context) {
            _userManager = userManager;
        }

        [BindProperty (SupportsGet = true)]
        public long Id { get; set; }

        public class InputModel {
            public InputModel () {
                BeginDate = DateTime.Now;
            }

            [Display (Name = "توضیحات : ")]
            public string Brief { get; set; }

            [Display (Name = "قالب : ")]
            [Required (ErrorMessage = ConstValues.RqError)]
            [Range (1, long.MaxValue, ErrorMessage = ConstValues.RgError)]
            public long TblTemplateId { get; set; }
            public List<SelectListItem> Templates { get; set; }

            [Display (Name = "کد توقف : ")]
            [Required (ErrorMessage = ConstValues.RqError)]
            public string StopCode { get; set; }

            [Display (Name = "شماره فنی : ")]
            public string TechnicalNumber { get; set; }

            [Display (Name = "نوع توقف : ")]
            [Required (ErrorMessage = ConstValues.RqError)]
            public StopType StopType { get; set; }

            public List<SelectListItem> StopTypes { get; set; }

            public DateTime BeginDate { set; get; }

            // 
            public string UserId { set; get; }
            public long TblStopId { get; set; }
            public long TblPlanningId { get; set; }
            public ScheduleType ScheduleType { get; set; }
            public bool IsAvailabel { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class ListModel {
            public long Id { get; set; }
            public string Template { get; set; }
            public string UserName { get; set; }

            // 
            public string StopType { get; set; }
            public string StopTitle { get; set; }
            public string StopCode { get; set; }

            // date
            public string BeginDate { get; set; }
            public bool IsModifyBeginDate { get; set; }
            public DateTime HelperBeginDate { get; set; }
            public string FinishDate { get; set; }
            public bool IsModifyFinishDate { get; set; }
            // 
            public int CalculatedMinutes { get; set; }
            public string TechnicalNumber { get; set; }
            public string Brief { get; set; }

            // 
            public bool IsStopped { get; set; }
        }

        public PaginatedList<ListModel> List { get; set; }

        public async Task<IActionResult> OnGetAsync (int p = 1) {
            if (!await _context.TblPlanning.AnyAsync (x => x.Id == Id)) {
                return NotFound ();
            }
            var username = HttpContext.User.Identity.GetUserName ();;
            var user = await _userManager.FindByNameAsync (username);
            var isCo = await _userManager.IsInRoleAsync (user, nameof (RoleType.CoRole));
            var isPManager = await _userManager.IsInRoleAsync (user, nameof (RoleType.ProductionManagerRole));
            var planningInfo = await _context.TblPlanning
                .Include (x => x.TblHall).SingleAsync (x => x.Id == Id);
            if (!isCo && !isPManager && user.TblHallId != planningInfo.TblHallId) {
                return Forbid ();
            }
            List = await PaginatedList<ListModel>.CreateAsync (
                _dbSet
                .Include (x => x.TblTemplate)
                .Include (x => x.TblStop)
                .Include (x => x.AppUser)
                .Where (x => x.TblPlanningId == Id)
                .OrderByDescending (x => x.Id)
                .Select (x => new ListModel {
                    Id = x.Id,
                        UserName = x.AppUser.UserName,
                        Template = x.TblTemplate.Code,
                        StopType = x.StopTitle,
                        StopCode = x.TblStop.Code,
                        BeginDate = x.PersianBeginDate,
                        HelperBeginDate = x.BeginDate,
                        FinishDate = x.PersianFinishDate,
                        StopTitle = x.TblStop.Title,
                        CalculatedMinutes = (int) x.CalculatedTime.TotalMinutes,
                        TechnicalNumber = x.TechnicalNumber,
                        Brief = x.Brief,
                        IsStopped = x.FinishDate.HasValue,
                        IsModifyBeginDate = x.ModifyBeginDate,
                        IsModifyFinishDate = x.ModifyFinishDate,
                }), p, _pageSize
            );
            return Page ();
        }

        public async Task<IActionResult> OnPostAsync () {
            if (ModelState.IsValid) {
                try {
                    var stopCode = Input.StopCode;
                    var stopType = Input.StopType;
                    var planning = await _context.TblPlanning
                        .Include (x => x.TblHall).SingleAsync (x => x.Id == Id);
                    var stop = await _context.TblStop
                        .FirstOrDefaultAsync (x => x.TblHallId == planning.TblHallId && x.Code == stopCode);
                    if (stop == null) {
                        throw new Exception ($"کد توقف '{stopCode}' مربوط به خط '{planning.TblHall.FullTitle}' نمی باشد.");
                    }
                    if ((stopType == StopType.TechCI || stopType == StopType.TechAl) &&
                        string.IsNullOrEmpty (Input.TechnicalNumber)) {
                        throw new Exception ("شماره فنی خالی می باشد.");
                    }
                    // 
                    // Set schedule type
                    // 
                    if (planning.ScheduleType == ScheduleType.Single) {
                        Input.ScheduleType = ScheduleType.Single;
                    } else {
                        var now = DateTime.Now;
                        var scheduleInfo = await _context.TblSchedule
                            .Where (x => x.ScheduleType == ScheduleType.Couple && x.SubsetId.HasValue).ToListAsync ();
                        var firstSchedule = scheduleInfo.FirstOrDefault ();
                        var currentTimeSpan = new TimeSpan (now.Hour, now.Minute, now.Second);
                        if (currentTimeSpan >= firstSchedule.BeginTime && currentTimeSpan <= firstSchedule.FinishTime) {
                            Input.ScheduleType = ScheduleType.Single;
                        } else {
                            Input.ScheduleType = ScheduleType.Couple;
                        }
                    }
                    Input.TblPlanningId = Id;
                    Input.TblStopId = stop.Id;
                    Input.UserId = HttpContext.User.Identity.GetUserId ();
                    await AddItemExtendNonSaveChange<InputModel> (Input);
                    await _ChangeStateHall (false);
                    var stopInfoToUpdate = await _dbSet
                        .Where (x => !x.ModifyBeginDate || !x.ModifyFinishDate && x.TblPlanningId == Id).ToListAsync ();
                    stopInfoToUpdate.ForEach (item => {
                        item.ModifyBeginDate = true;
                        item.ModifyFinishDate = true;
                    });
                    _context.TblStopInfo.UpdateRange (stopInfoToUpdate);
                    await _context.SaveChangesAsync ();
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
            if (await _dbSet.AnyAsync (x => x.TblPlanningId == Id && !x.FinishDate.HasValue)) {
                return Partial ("_Create", model);
            }
            var planning = await _context.TblPlanning
                .Include (x => x.TblHall).FirstOrDefaultAsync (x => x.Id == Id);
            var hallType = planning.TblHall.HallType;

            Func<StopType, bool> condition = x => hallType == HallType.CastIron ?
                (x == StopType.TechCI || x == StopType.NonTechCI) : (x != StopType.TechCI && x != StopType.NonTechCI);

            model.StopTypes = Enum.GetValues (typeof (StopType)).OfType<StopType> ()
                .Where (condition).Select (x =>
                    new SelectListItem {
                        Text = x.GetDisplayName (),
                            Value = (Convert.ToInt32 (x)).ToString ()
                    }).ToList ();
            model.IsAvailabel = true;
            model.Templates = await _FetchAsSelectAsync ();
            return Partial ("_Create", model);
        }

        public async Task<IActionResult> OnPostStartAsync (long stopId) {
            var item = await _dbSet.FindAsync (stopId);
            item.FinishDate = DateTime.Now;
            _context.TblStopInfo.Update (item);
            await _ChangeStateHall (true);
            await _context.SaveChangesAsync ();
            return RedirectToPage ("./Index");
        }

        public async Task<IActionResult> OnPostSubtractBeginAsync (long bid, int time) {
            try {
                if (time > -6 && time < 6) {
                    var currentItem = await _dbSet.FindAsync (bid);
                    var previousItem = await _dbSet.Where (x => x.Id < currentItem.Id && x.TblPlanningId == Id)
                        .OrderByDescending (x => x.Id).FirstOrDefaultAsync ();
                    var currentValue = currentItem.BeginDate.AddMinutes (time);
                    if (currentValue > currentItem.FinishDate) {
                        throw new Exception ($@"تاریخ <bdo class='ltr'>{currentValue.ToShortPersianDateTimeString()}</bdo> نمی تواند بزرگتر از <bdo class='ltr'>
                        {currentItem.PersianFinishDate}</bdo> (توقف فعلی) باشد.");
                    }
                    if (currentValue <= previousItem?.FinishDate.Value) {
                        throw new Exception ($@"تاریخ <bdo class='ltr'>{currentValue.ToShortPersianDateTimeString ()}</bdo>
                         نمی تواند کوچکتر از <bdo class='ltr'>{previousItem.FinishDate.Value.ToShortPersianDateTimeString ()}</bdo> (توقف قبلی) باشد.");
                    }
                    currentItem.ModifyBeginDate = true;
                    currentItem.BeginDate = currentItem.BeginDate.AddMinutes (time);
                    await EditItem (currentItem);
                    Alert = ModelStateType.A200.ModelStateAsText ();
                } else {
                    throw new Exception (RangeError);
                }
            } catch (Exception ex) {
                ModelState.Clear ();
                ModelState.AddModelError ("", ex.Message);
                Alert = ModelState.ModelStateAsError ();
            }
            return RedirectToPage ("./Index");
        }

        public async Task<IActionResult> OnPostSubtractFinishAsync (long fid, int time) {
            try {
                if (time > -6 && time < 6) {
                    var currentItem = await _dbSet.FindAsync (fid);
                    var currentValue = currentItem.FinishDate.Value.AddMinutes (time);
                    if (currentValue < currentItem.BeginDate) {
                        throw new Exception ($@"تاریخ <bdo class='ltr'>{currentValue.ToShortPersianDateTimeString()}</bdo> (پایان) نمی تواند کوچکتر از <bdo class='ltr'>
                        {currentItem.PersianBeginDate}</bdo> (شروع) باشد.");
                    }
                    currentItem.ModifyFinishDate = true;
                    currentItem.FinishDate = currentValue;
                    await EditItem (currentItem);
                    Alert = ModelStateType.A200.ModelStateAsText ();
                } else {
                    throw new Exception (RangeError);
                }
            } catch (Exception ex) {
                ModelState.Clear ();
                ModelState.AddModelError ("", ex.Message);
                Alert = ModelState.ModelStateAsError ();
            }
            return RedirectToPage ("./Index");
        }

        private async Task _ChangeStateHall (bool state) {
            var planning = await _context.TblPlanning
                .FirstOrDefaultAsync (x => x.Id == Id);
            var hall = await _context.TblHall.FindAsync (planning.TblHallId);
            hall.IsWorking = state;
            _context.TblHall.Update (hall);
        }

        private async Task<List<SelectListItem>> _FetchAsSelectAsync () {
            var result = await _context
                .TblTemplate.
            Include (x => x.TblProductionInfo.Where (x => x.TblPlanningId == Id))
                .Include (x => x.TblStopInfo.Where (x => x.TblPlanningId == Id))
                .Where (x => x.TblPlanningInfo.Any (y => y.TblPlanningId == Id) &&
                    !x.TblStopInfo.Any (z => z.TblPlanningId == Id && !z.FinishDate.HasValue))
                .Select (x => new SelectListItem {
                    Value = x.Id.ToString (),
                        Text = x.Code,
                }).ToListAsync ();
            result.Insert (0, new SelectListItem () { Value = "0", Text = "انتخاب کنید" });
            return result;
        }
    }
}
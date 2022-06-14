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
using DbLayer.Context;
using DbLayer.DbTable;
using DbLayer.DbTable.Identity;
using DbLayer.Enums;
using ZyPanel.Areas.Shared;
using ZyPanel.Helper.Utils;
using ZyPanel.Service.IdentityService;

namespace ZyPanel.Areas.Production.Pages.ToMake {
    public class IndexModel : ModifyRootPage<TblProductionInfo> {
        private readonly UserManager<AppUser> _userManager;

        public IndexModel (AppDbContext context, UserManager<AppUser> userManager) : base (context) {
            _userManager = userManager;
        }

        [BindProperty (SupportsGet = true)]
        public long Id { get; set; }

        public bool IsStopped { get; set; }

        public bool CanCreate { get; set; }

        public class InputModel {
            public bool IsCastIron { get; set; }

            [Display (Name = "تعداد بار ریزی شده : ")]
            [Required (ErrorMessage = ConstValues.RqError)]
            [Range (0, int.MaxValue, ErrorMessage = ConstValues.RgError)]
            public int PutCount { get; set; }

            [Display (Name = "تعداد گرفته شده : ")]
            [Required (ErrorMessage = ConstValues.RqError)]
            [Range (1, int.MaxValue, ErrorMessage = ConstValues.RgError)]
            public int TakeCount { get; set; }

            [Display (Name = "قالب : ")]
            [Required (ErrorMessage = ConstValues.RqError)]
            [Range (1, long.MaxValue, ErrorMessage = ConstValues.RgError)]
            public long TblTemplateId { get; set; }
            public List<SelectListItem> Templates { get; set; }

            [Display (Name = "نوع بارریزی : ")]
            public ChargeType ChargeType { get; set; } = ChargeType.Default;

            // 
            public long HallId { get; set; }
            public string UserId { get; set; }
            public string WeightCode { get; set; }
            public long TblPlanningId { get; set; }
            public ScheduleType ScheduleType { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class ListModel {
            public long Id { get; set; }

            public int PutCount { get; set; }

            public int TakeCount { get; set; }

            public string WeightCode { get; set; }

            public string TemplateCode { get; set; }

            public double ProductWeightAsKg { get; set; }

            public double TemplateWeightAsKg { get; set; }
        }

        public List<ListModel> List { get; set; }

        public async Task<IActionResult> OnGetAsync (int p = 1) {
            if (!await _context.TblPlanning.AnyAsync (x => x.Id == Id)) {
                return NotFound ();
            }
            var username = HttpContext.User.Identity.GetUserName ();
            var user = await _userManager.FindByNameAsync (username);
            var isCo = await _userManager.IsInRoleAsync (user, nameof (RoleType.CoRole));
            var isPManager = await _userManager.IsInRoleAsync (user, nameof (RoleType.ProductionManagerRole));
            var planningInfo = await _context.TblPlanning
                .Include (x => x.TblHall).SingleAsync (x => x.Id == Id);
            if (!isCo && !isPManager && user.TblHallId != planningInfo.TblHallId) {
                return Forbid ();
            }
            var now = DateTime.Now;
            CanCreate = now.Date >= planningInfo.BeginDate.Date && now.Date <= planningInfo.FinishDate.Date;
            IsStopped = await _context.TblStopInfo.AnyAsync (x => x.TblPlanningId == Id && !x.FinishDate.HasValue);
            // 
            // 
            // 
            List = await PaginatedList<ListModel>.CreateAsync (
                _dbSet.Where (x => x.TblPlanningId == Id)
                .GroupBy (x => new { x.TblTemplateId, x.WeightCode })
                .Select (x => new ListModel {
                    WeightCode = x.Key.WeightCode,
                        TakeCount = x.Sum (x => x.TakeCount),
                        PutCount = x.Sum (x => x.PutCount),
                }), p, _pageSize
            );
            var templateWeightCodes = List.Select (x => x.WeightCode).ToArray ();
            var joinTp = await _context.JoinTP
                .Where (x => templateWeightCodes.Any (y => y == x.WeightCode))
                .Include (x => x.TblTemplate).Select (x => new {
                    TemplateCode = x.TblTemplate.Code,
                        WeightCode = x.WeightCode,
                        TemplateWeightAsKg = x.TemplateWeightAsKg,
                        ProductWeightAsKg = x.ProductWeightAsKg,
                }).ToListAsync ();
            foreach (var item in List) {
                item.TemplateCode = joinTp.FirstOrDefault (x => x.WeightCode == item.WeightCode).TemplateCode;
                item.TemplateWeightAsKg = joinTp.FirstOrDefault (x => x.WeightCode == item.WeightCode).TemplateWeightAsKg;
                item.ProductWeightAsKg = joinTp.Where (x => x.WeightCode == item.WeightCode).Sum (x => x.ProductWeightAsKg);
            }
            return Page ();
        }

        public async Task<IActionResult> OnPostAsync () {
            if (ModelState.IsValid) {
                try {
                    if (Input.TakeCount < Input.PutCount) {
                        throw new Exception ("تعداد گرفته شده از تعداد بارریزی نمی تواند کمترباشد.");
                    }
                    var now = DateTime.Now;
                    var planning = await _context.TblPlanning.FindAsync (Id);
                    if (planning.ScheduleType == ScheduleType.Single) {
                        Input.ScheduleType = ScheduleType.Single;
                    } else {
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
                    Input.TblPlanningId = planning.Id;
                    var joinTP = await _context.JoinTP.FirstOrDefaultAsync (x => x.TblTemplateId == Input.TblTemplateId && x.IsReady);
                    Input.WeightCode = joinTP.WeightCode;
                    Input.UserId = HttpContext.User.Identity.GetUserId ();
                    if (!Input.IsCastIron) {
                        Input.PutCount = Input.TakeCount;
                    }
                    Input.HallId = planning.TblHallId;
                    await AddItemExtend<InputModel> (Input);
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
            var finalItems = new List<SelectListItem> ();
            var planning = await _context.TblPlanning
                .Include (x => x.TblHall).SingleAsync (x => x.Id == Id);
            model.IsCastIron = planning.TblHall.HallType == HallType.CastIron ? true : false;
            // The templates that were chose by 'Planning'.
            var planningTemplates = await _context.TblTemplate
                .Where (x => x.TblPlanningInfo.Any (x => x.TblPlanningId == Id))
                .Include (x => x.TblPlanningInfo.Where (x => x.TblPlanningId == Id))
                .Include (x => x.JoinTP.Where (j => j.IsReady))
                .Select (x => new HelperTemplate {
                    TemplateId = x.Id,
                        TemplateCode = x.Code,
                        TemplateKaviteCount = x.KaviteCount,
                        ProducIds = x.JoinTP.Where (j => j.IsReady).Select (x => x.TblProductId).ToList ()
                }).ToListAsync ();
            foreach (var item in planningTemplates) {
                finalItems.Add (new SelectListItem {
                    Value = item.TemplateId.ToString (),
                        Text = item.TemplateCode
                });
            }
            var templateIds = planningTemplates.Select (x => x.TemplateId).ToArray ();
            var templateKavites = planningTemplates.Select (x => (int) x.TemplateKaviteCount).ToArray ();
            // The templates that maybe be alternative.
            var alternativeTemplates = await _context.TblTemplate
                .Include (x => x.TblHallTemplate)
                .Include (x => x.JoinTP.Where (j => j.IsReady && !templateIds.Any (t => t == j.TblTemplateId)))
                .Where (x => x.IsActive &&
                    !templateIds.Any (i => i == x.Id) &&
                    templateKavites.Any (i => i == (int) x.KaviteCount) &&
                    x.JoinTP.Any (j => j.IsReady && !templateIds.Any (t => t == j.TblTemplateId)) &&
                    x.TblHallTemplate.Any (y => y.TblHallId == planning.TblHallId)
                ).Select (x => new HelperTemplate {
                    TemplateId = x.Id,
                        TemplateCode = x.Code,
                        TemplateKaviteCount = x.KaviteCount,
                        ProducIds = x.JoinTP.Where (j => j.IsReady).Select (x => x.TblProductId).ToList ()
                }).ToListAsync ();
            foreach (var item in alternativeTemplates) {
                if (planningTemplates.Any (x => x.ProducIds.SequenceEqual (item.ProducIds))) {
                    finalItems.Add (new SelectListItem {
                        Value = item.TemplateId.ToString (),
                            Text = $"{item.TemplateCode}-پیشنهاد جایگزین"
                    });
                }
            }
            finalItems.Insert (0, new SelectListItem () { Value = "0", Text = "انتخاب کنید" });
            model.Templates = finalItems;
            return Partial ("_Create", model);
        }
    }

    public class HelperTemplate {
        public long TemplateId { get; set; }
        public string TemplateCode { get; set; }
        public byte TemplateKaviteCount { get; set; }
        public List<long> ProducIds { get; set; }
    }
}
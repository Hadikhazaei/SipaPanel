using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

// 
using CldLayer.Persian;
using DbLayer.Context;
using DbLayer.DbTable.Identity;
using DbLayer.Enums;
using ZyPanel.Service.IdentityService;

namespace ZyPanel.Areas.Production.Pages {
    public class IndexModel : PageModel {
        public readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public IndexModel (AppDbContext context, UserManager<AppUser> userManager) {
            _context = context;
            _userManager = userManager;
        }

        public string Date { get; set; }

        public class ListModel {
            // hall
            public bool IsWorking { get; set; }

            public string HallTitle { get; set; }

            public List<TemplateModel> Templates { get; set; }
        }

        public class TemplateModel {
            // template
            public string TemplateCode { get; set; }
            public int SingleSchedulePutCount { get; set; }
            public int CoupleSchedulePutCount { get; set; }

            // stop
            public int SingleStopTime { get; set; }

            public int CoupleStopTime { get; set; }

            public List<KeyValuePair<long, string>> Products { get; set; }
        }

        public List<ListModel> List { get; set; }

        public async Task<IActionResult> OnGetAsync (string date) {
            var result = new List<ListModel> ();
            var tomorrow = DateTime.Now.AddDays (1);
            var yesterday = DateTime.Now.AddDays (-1);
            Date = DateTime.Now.ToPersianDateTextify ();
            if (!string.IsNullOrEmpty (date)) {
                var gregorian = date.ToGregorianDateTimeOrDefault ();
                tomorrow = gregorian.AddDays (1);
                yesterday = gregorian.AddDays (-1);
                Date = gregorian.ToPersianDateTextify ();
            }
            var userId = HttpContext.User.Identity.GetUserId ();
            var user = await _userManager.FindByIdAsync (userId);
            // 
            // productionInfo
            // 
            var todayProductionInfo = await _context.TblProductionInfo
                .Include (x => x.TblTemplate)
                .Where (x => x.CreatedDate.Date > yesterday.Date &&
                    x.CreatedDate.Date < tomorrow.Date &&
                    x.HallId == user.TblHallId)
                .Select (x => new {
                    Id = x.Id,
                        HallId = x.HallId,
                        PutCount = x.PutCount,
                        WeightCode = x.WeightCode,
                        ScheduleType = x.ScheduleType,
                        TemplateId = x.TblTemplateId,
                        TemplateCode = x.TblTemplate.Code,
                }).ToListAsync ();
            var templatesId = todayProductionInfo.GroupBy (x => x.TemplateId).Select (x => x.Key).ToArray ();
            // 
            // stopInfo
            // 
            var todayStopInfo = await _context.TblStopInfo
                .Where (x => x.BeginDate.Date > yesterday.Date &&
                    x.FinishDate.Value.Date < tomorrow.Date &&
                    templatesId.Any (t => t == x.TblTemplateId))
                .Select (x => new {
                    TemplateId = x.TblTemplateId,
                        ScheduleType = x.ScheduleType,
                        CalculatedMinutes = x.CalculatedTime
                }).ToListAsync ();
            // 
            // templateInfo
            // 
            var templateAndWeightCode = todayProductionInfo
                .GroupBy (x => new { x.TemplateId, x.WeightCode })
                .Select (x => x.Key).ToList ();
            var tIds = templateAndWeightCode.Select (x => x.TemplateId).ToArray ();
            var tWCodes = templateAndWeightCode.Select (x => x.WeightCode).ToArray ();
            var todayTemplateInfo = await _context.JoinTP
                .Include (x => x.TblProduct)
                .Include (x => x.TblTemplate)
                .ThenInclude (x => x.TblHallTemplate)
                .Where (x => tIds.Any (y => y == x.TblTemplateId) &&
                    tWCodes.Any (w => w == x.WeightCode))
                .Select (x => new {
                    TemplateId = x.TblTemplateId,
                        ProductCount = x.ProductCount,
                        ProductTitle = x.TblProduct.Title,
                }).ToListAsync ();
            // 
            // 
            // 
            var hallsId = todayProductionInfo.GroupBy (x => x.HallId).Select (x => x.Key).ToArray ();
            var todayHallsInfo = await _context.TblHall.Where (x => hallsId.Any (h => h == x.Id)).ToListAsync ();
            foreach (var hallId in hallsId) {
                var itemResult = new ListModel ();
                var templates = new List<TemplateModel> ();
                // 
                // fill hall data
                // 
                itemResult.HallTitle = todayHallsInfo.FirstOrDefault (x => x.Id == hallId).FullTitle;
                itemResult.IsWorking = todayHallsInfo.FirstOrDefault (x => x.Id == hallId).IsWorking;
                // 
                // 
                // 
                var templateUnique = todayProductionInfo.Where (x => x.HallId == hallId)
                    .GroupBy (x => x.TemplateId).Select (x => x.Key).ToArray ();
                foreach (var templateId in templateUnique) {
                    var itemTemplate = new TemplateModel ();
                    // 
                    // fill template data
                    // 
                    itemTemplate.TemplateCode = todayProductionInfo
                        .FirstOrDefault (x => x.HallId == hallId && x.TemplateId == templateId).TemplateCode;
                    itemTemplate.SingleSchedulePutCount = todayProductionInfo
                        .Where (x => x.HallId == hallId &&
                            x.ScheduleType == ScheduleType.Single &&
                            x.TemplateId == templateId).Sum (x => x.PutCount);
                    itemTemplate.CoupleSchedulePutCount = todayProductionInfo
                        .Where (x => x.HallId == hallId &&
                            x.ScheduleType == ScheduleType.Couple &&
                            x.TemplateId == templateId).Sum (x => x.PutCount);
                    // 
                    // fill stop data
                    // 
                    itemTemplate.SingleStopTime = todayStopInfo
                        .Where (x => x.TemplateId == templateId &&
                            x.ScheduleType == ScheduleType.Single &&
                            x.TemplateId == templateId).Sum (x => (int) x.CalculatedMinutes.TotalMinutes);
                    itemTemplate.CoupleStopTime = todayStopInfo
                        .Where (x => x.TemplateId == templateId &&
                            x.ScheduleType == ScheduleType.Couple &&
                            x.TemplateId == templateId).Sum (x => (int) x.CalculatedMinutes.TotalMinutes);
                    // 
                    // fill product data
                    // 
                    itemTemplate.Products = todayTemplateInfo
                        .Where (x => x.TemplateId == templateId)
                        .Select (x => new { key = x.ProductCount, value = x.ProductTitle })
                        .Select (x => new KeyValuePair<long, string> (x.key, x.value)).ToList ();
                    // 
                    // 
                    // 
                    templates.Add (itemTemplate);
                }
                itemResult.Templates = templates;
                result.Add (itemResult);
            }
            List = result;
            return Page ();
        }
    }
}
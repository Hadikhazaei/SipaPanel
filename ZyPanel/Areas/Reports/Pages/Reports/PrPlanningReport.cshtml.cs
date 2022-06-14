using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

// 
using CldLayer.Persian;
using DbLayer.Context;
using ZyPanel.Helper.Utils;

namespace ZyPanel.Areas.Reports.Pages.Reports {
    public class PrPlanningReportModel : PageModel {
        public readonly AppDbContext _context;
        public PrPlanningReportModel (AppDbContext context) {
            _context = context;
        }

        public class ListModel {
            public string HallTitle { get; set; }
            public DetailModel Daily { get; set; }
            public DetailModel WeekLy { get; set; }
            public DetailModel MonthLy { get; set; }
            public DetailModel YearLy { get; set; }
        }

        public class DetailModel {
            public string BeginDate { get; set; }
            public string FinishDate { get; set; }
            public string PlanningValue { get; set; }
            public string ProductionValue { get; set; }
            public string DifferentValue { get; set; } = "---";
        }

        public List<ListModel> List { get; set; }

        public async Task<IActionResult> OnGetAsync (string date) {
            // calculate the today
            var today = DateTime.Now;
            if (!string.IsNullOrEmpty (date)) {
                today = date.ToGregorianDateTimeOrDefault ();
            }
            // calculate the week
            var startAndEndWeek = StaticHelper.GetStartAndEndWeek (today);
            var startWeek = startAndEndWeek.Item1;
            var finishWeek = startAndEndWeek.Item2;
            // calculate the month
            var dayOfMonth = today.GetPersianDayOfMonth ();
            var startMonth = today.AddDays (-(dayOfMonth - 1));
            var daysOfMonth = today.GetPersianMonth () > 6 ? 29 : 30;
            var finishMonth = startMonth.AddDays (daysOfMonth);
            // calculate the year
            var hijriYear = today.GetPersianYear ();
            var aYearAgo = $"{hijriYear}/01/01".ToGregorianDateTimeOrDefault ();
            // 
            // planning
            // 
            var planningInfo = await _context.TblPlanning
                .Include (x => x.TblHall)
                .Where (x => x.BeginDate <= today && x.FinishDate >= aYearAgo)
                .AsNoTracking ().Select (x => new {
                    Id = x.Id,
                        HallId = x.TblHallId,
                        BeginDate = x.BeginDate,
                        FinishDate = x.FinishDate,
                        HallTitle = x.TblHall.FullTitle,
                }).ToListAsync ();
            var planningIds = planningInfo.GroupBy (x => x.Id).Select (x => x.Key).ToArray ();
            var tonnageInfo = await _context.TblTonnage
                .Where (x => planningIds.Any (y => y == x.TblPlanningId))
                .AsNoTracking ().Select (x => new {
                    PlanningId = x.TblPlanningId,
                        TonnageDate = x.TonnageDate,
                        UsedTonnageAsKg = x.UsedTonnageAsKg
                }).ToListAsync ();
            // 
            // production
            // 
            var productionInfo = await _context.TblProductionInfo
                .Where (x => planningIds.Any (y => y == x.TblPlanningId))
                .AsNoTracking ().Select (x => new {
                    WeightCode = x.WeightCode,
                        PutCount = x.PutCount,
                        CreatedDate = x.CreatedDate,
                        PlanningId = x.TblPlanningId,
                }).ToListAsync ();
            var weightCodes = productionInfo.GroupBy (x => x.WeightCode).Select (x => x.Key).ToArray ();
            // 
            // template
            // 
            var joinTpInfo = await _context.JoinTP
                .Where (x => weightCodes.Any (y => y == x.WeightCode))
                .AsNoTracking ().Select (x => new {
                    WeightCode = x.WeightCode, WeightAsKg = x.TemplateWeightAsKg
                }).ToListAsync ();
            // 
            // seperated by line id
            // 
            var result = new List<ListModel> ();
            var hallIds = planningInfo.GroupBy (x => x.HallId).Select (x => x.Key).ToArray ();
            foreach (var item in hallIds) {
                var listItem = new ListModel ();
                listItem.HallTitle = planningInfo.FirstOrDefault (x => x.HallId == item).HallTitle;
                var planningOfHallIds = planningInfo.Where (x => x.HallId == item).Select (x => x.Id).ToArray ();
                var tonnage = tonnageInfo.Where (x => planningOfHallIds.Any (y => y == x.PlanningId)).ToList ();
                var production = productionInfo.Where (x => planningOfHallIds.Any (y => y == x.PlanningId)).ToList ();

                // daily
                var dailyTemp = production.Where (x => x.CreatedDate.Date == today.Date).ToList ();
                var dailyPutCount = dailyTemp.Sum (x => x.PutCount);
                var dailyWeightCodes = dailyTemp.Select (x => x.WeightCode).ToArray ();
                var dailyPlanningValueAsKg = tonnage.Where (x => x.TonnageDate.Date == today.Date).Sum (x => x.UsedTonnageAsKg);
                var dailyProductionValueAsKg = joinTpInfo.Where (x => dailyWeightCodes.Any (y => y == x.WeightCode)).Sum (x => x.WeightAsKg);
                var dailyVm = new DetailModel ();
                var dailyPlTone = (double) dailyPlanningValueAsKg / 1000;
                var dailyPrTone = (double) (dailyPutCount * dailyProductionValueAsKg) / 1000;
                dailyVm.BeginDate = today.ToShortPersianDateString ();
                dailyVm.FinishDate = today.ToShortPersianDateString ();
                dailyVm.PlanningValue = dailyPlTone.ToString ("0.##");
                dailyVm.ProductionValue = dailyPrTone.ToString ("0.##");
                if (dailyPlTone != 0) {
                    var dailyPercent = FormatHelper.GetPercentValue (dailyPrTone, dailyPlTone - 100);
                    dailyVm.DifferentValue = FormatHelper.GetPercentFormat (dailyPercent);
                }
                listItem.Daily = dailyVm;

                // weekly
                var weeklyTemp = production
                    .Where (x => x.CreatedDate.Date >= startWeek.Date && x.CreatedDate <= finishWeek.Date).ToList ();
                var weeklyPutCount = weeklyTemp.Sum (x => x.PutCount);
                var weeklyWeightCodes = weeklyTemp.Select (x => x.WeightCode).ToArray ();
                var weeklyPlanningValueAsKg = tonnage
                    .Where (x => x.TonnageDate.Date >= startWeek.Date && x.TonnageDate <= finishWeek.Date).Sum (x => x.UsedTonnageAsKg);
                var weeklyProductionValueAsKg = joinTpInfo.Where (x => weeklyWeightCodes.Any (y => y == x.WeightCode)).Sum (x => x.WeightAsKg);
                var weeklyVm = new DetailModel ();
                var weeklyPlTone = (double) weeklyPlanningValueAsKg / 1000;
                var weeklyPrTone = (double) (weeklyPutCount * weeklyProductionValueAsKg) / 1000;
                weeklyVm.BeginDate = startWeek.ToShortPersianDateString ();
                weeklyVm.FinishDate = finishWeek.ToShortPersianDateString ();
                weeklyVm.PlanningValue = weeklyPlTone.ToString ("0.##");
                weeklyVm.ProductionValue = weeklyPrTone.ToString ("0.##");
                if (weeklyPlTone != 0) {
                    var weeklyPercent = FormatHelper.GetPercentValue (weeklyPrTone, weeklyPlTone - 100);
                    weeklyVm.DifferentValue = FormatHelper.GetPercentFormat (weeklyPercent);
                }
                listItem.WeekLy = weeklyVm;

                // monthly
                var monthlyTemp = production
                    .Where (x => x.CreatedDate.Date >= startMonth.Date && x.CreatedDate <= finishMonth.Date).ToList ();
                var monthlyPutCount = monthlyTemp.Sum (x => x.PutCount);
                var monthlyWeightCodes = monthlyTemp.Select (x => x.WeightCode).ToArray ();
                var monthlyProductionValue = joinTpInfo.Where (x => monthlyWeightCodes.Any (y => y == x.WeightCode)).Sum (x => x.WeightAsKg);
                var monthlyPlanningValueAsKg = tonnage
                    .Where (x => x.TonnageDate.Date >= startMonth.Date && x.TonnageDate <= finishMonth.Date).Sum (x => x.UsedTonnageAsKg);
                var monthlyVm = new DetailModel ();
                var monthlyPlTone = (double) monthlyPlanningValueAsKg / 1000;
                var monthlyPrTone = (double) (monthlyPutCount * monthlyProductionValue) / 1000;
                monthlyVm.BeginDate = startMonth.ToShortPersianDateString ();
                monthlyVm.FinishDate = finishMonth.ToShortPersianDateString ();
                monthlyVm.PlanningValue = monthlyPlTone.ToString ("0.##");
                monthlyVm.ProductionValue = monthlyPrTone.ToString ("0.##");
                if (monthlyPlTone != 0) {
                    var monthlyPercent = FormatHelper.GetPercentValue (monthlyPrTone, monthlyPlTone - 100);
                    monthlyVm.DifferentValue = FormatHelper.GetPercentFormat (monthlyPercent);
                }
                listItem.MonthLy = monthlyVm;

                // yearly
                var yearlyTemp = production.Where (x => x.CreatedDate >= aYearAgo.Date).ToList ();
                var yearlyPutCount = yearlyTemp.Sum (x => x.PutCount);
                var yearlyWeightCodes = yearlyTemp.Select (x => x.WeightCode).ToArray ();
                var yearlyProductionValue = joinTpInfo.Where (x => yearlyWeightCodes.Any (y => y == x.WeightCode)).Sum (x => x.WeightAsKg);
                var yearlyPlanningValueKg = tonnage.Where (x => x.TonnageDate >= aYearAgo.Date).Sum (x => x.UsedTonnageAsKg);
                var yearlyVm = new DetailModel ();
                var yearlyPlTone = (double) yearlyPlanningValueKg / 1000;
                var yearlyPrTone = (double) (yearlyPutCount * yearlyProductionValue) / 1000;
                yearlyVm.BeginDate = aYearAgo.ToShortPersianDateString ();
                yearlyVm.FinishDate = aYearAgo.AddYears (1).ToShortPersianDateString ();
                yearlyVm.PlanningValue = yearlyPlTone.ToString ("0.##");
                yearlyVm.ProductionValue = yearlyPrTone.ToString ("0.##");
                if (yearlyPlTone != 0) {
                    var yearlyPercent = FormatHelper.GetPercentValue (yearlyPrTone, yearlyPlTone - 100);
                    yearlyVm.DifferentValue = FormatHelper.GetPercentFormat (yearlyPercent);
                }
                listItem.YearLy = yearlyVm;
                // 
                result.Add (listItem);
            }
            List = result;
            return Page ();
        }

    }
}
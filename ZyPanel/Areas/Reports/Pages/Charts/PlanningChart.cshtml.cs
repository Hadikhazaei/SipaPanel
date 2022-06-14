using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

// 
using CldLayer.Persian;
using DbLayer.Context;
using ZyPanel.Helper.Vmodel;

namespace ZyPanel.Areas.Reports.Pages.Charts {
    public class PlanningChartModel : PageModel {

        public readonly AppDbContext _context;

        public PlanningChartModel (AppDbContext context) { _context = context; }

        public LineChartVm YearlyData { get; set; }

        public LineChartVm MonthlyData { get; set; }

        public int StartYear => 1400;
        public int CurrentYear => PersianCulture.GetPersianYear (DateTime.Now);

        [BindProperty (SupportsGet = true)]
        public int? LineId { get; set; }
        public List<SelectListItem> Lines { get; set; }

        [BindProperty (SupportsGet = true)]
        public int? Year { get; set; }

        [BindProperty (SupportsGet = true)]
        public int? Month { get; set; }

        public async Task<IActionResult> OnGetAsync () {
            // 
            // 
            // 
            Lines = await _FetchLinesAsSelectAsync ();
            LineId = LineId ?? int.Parse (Lines.First ().Value);
            // 
            // 
            // 
            Year = Year ?? this.CurrentYear;
            Month = Month ?? DateTime.Now.GetPersianMonth ();
            // 
            // 
            // 
            await fillYearlyChartAsync ();
            await fillMonthlyChartAsync ();
            return Page ();
        }

        private async Task fillYearlyChartAsync () {
            var lineChart = new LineChartVm ();
            var persianDate = PersianCulture.GetPersianYearStartAndEndDates (Year.Value);
            // 
            // Planning
            // 
            var planningIds = await _context.TblPlanning
                .Where (x => x.TblHallId == LineId && x.BeginDate >= persianDate.StartDate &&
                    x.FinishDate <= persianDate.EndDate).Select (x => x.Id).ToListAsync ();
            // 
            // Tonnage
            // 
            var tonnage = await _context.TblTonnage
                .Where (x => planningIds.Any (y => y == x.TblPlanningId))
                .GroupBy (x => x.ShamsiDate.Month).Select (x => new {
                    Month = x.Key,
                        WeightAsKg = x.Sum (k => k.UsedTonnageAsKg)
                }).ToListAsync ();
            // 
            // Production / Template
            // 
            var production = await _context.TblProductionInfo
                .Where (x => planningIds.Any (y => y == x.TblPlanningId))
                .GroupBy (x => new { x.ShamsiDate.Month, x.WeightCode }).Select (x => new {
                    Month = x.Key.Month,
                        WeightCode = x.Key.WeightCode,
                        TemplateCounts = x.Sum (x => x.PutCount),
                }).ToListAsync ();
            var weightCodes = production.Select (x => x.WeightCode).ToArray ();
            var joinTp = await _context.JoinTP
                .Where (x => weightCodes.Any (y => y == x.WeightCode))
                .Select (x => new {
                    WeightCode = x.WeightCode,
                        TemplateWeightAsKg = x.TemplateWeightAsKg
                }).ToListAsync ();
            // 
            // Calculate
            // 
            var lineChartItems = new List<LineChartItemVm> ();
            for (var i = 1; i <= 12; i++) {
                var newItem = new LineChartItemVm ();
                newItem.DataNumber = i;
                double productValue = default;
                var lineValues = new List<string> ();
                newItem.DataTitle = PersianCulture.GetPersianMonthName (i);
                lineValues.Add (tonnage.Where (x => x.Month == i).Sum (x => x.WeightAsKg).ToString ());
                foreach (var item in production.Where (x => x.Month == i).ToList ()) {
                    var code = item.WeightCode;
                    var templateCount = item.TemplateCounts;
                    var templateWeight = joinTp.FirstOrDefault (x => x.WeightCode == code).TemplateWeightAsKg;
                    productValue += (double) templateCount * templateWeight;
                }
                lineValues.Add (productValue.ToString ());
                newItem.DataValue = string.Join (",", lineValues);
                lineChartItems.Add (newItem);
            }
            lineChart.SeriesTitle = string.Join (',', new string[] { "تولید", "برنامه" });
            lineChart.Series = lineChartItems.OrderBy (x => x.DataNumber).ToList ();
            YearlyData = lineChart;
        }

        private async Task fillMonthlyChartAsync () {
            var lineChart = new LineChartVm ();
            var persianDate = PersianCulture.GetPersianMonthStartAndEndDates (Year.Value, Month.Value);
            // 
            // Planning
            // 
            var planningIds = await _context.TblPlanning
                .Where (x => x.TblHallId == LineId && x.BeginDate >= persianDate.StartDate &&
                    x.FinishDate <= persianDate.EndDate).Select (x => x.Id).ToListAsync ();
            // 
            // Tonnage
            // 
            var tonnage = await _context.TblTonnage
                .Where (x => planningIds.Any (y => y == x.TblPlanningId))
                .GroupBy (x => x.ShamsiDate.Day)
                .Select (x => new {
                    Day = x.Key, WeightAsKg = x.Sum (k => k.UsedTonnageAsKg)
                }).ToListAsync ();
            // 
            // Production / Template
            // 
            var production = await _context.TblProductionInfo
                .Where (x => planningIds.Any (y => y == x.TblPlanningId))
                .GroupBy (x => new { x.ShamsiDate.Day, x.WeightCode }).Select (x => new {
                    Day = x.Key.Day,
                        WeightCode = x.Key.WeightCode,
                        TemplateCounts = x.Sum (x => x.PutCount)
                }).ToListAsync ();
            var weightCodes = production.Select (x => x.WeightCode).ToArray ();
            var joinTp = await _context.JoinTP
                .Where (x => weightCodes.Any (y => y == x.WeightCode))
                .Select (x => new {
                    WeightCode = x.WeightCode,
                        TemplateWeightAsKg = x.TemplateWeightAsKg
                }).ToListAsync ();
            // 
            // Calculate
            // 
            var lineChartItems = new List<LineChartItemVm> ();
            var daysCount = Math.Ceiling ((persianDate.EndDate - persianDate.StartDate).TotalDays);
            for (var i = 1; i <= daysCount; i++) {
                var newItem = new LineChartItemVm ();
                newItem.DataNumber = i;
                double productValue = default;
                newItem.DataTitle = i.ToString ();
                var lineValues = new List<string> ();
                lineValues.Add (tonnage.Where (x => x.Day == i).Sum (x => x.WeightAsKg).ToString ());
                foreach (var item in production.Where (x => x.Day == i).ToList ()) {
                    var code = item.WeightCode;
                    var templateCount = item.TemplateCounts;
                    var templateWeight = joinTp.FirstOrDefault (x => x.WeightCode == code).TemplateWeightAsKg;
                    productValue += (double) templateCount * templateWeight;
                }
                lineValues.Add (productValue.ToString ());
                newItem.DataValue = string.Join (",", lineValues);
                lineChartItems.Add (newItem);
            }
            lineChart.SeriesTitle = string.Join (',', new string[] { "تولید", "برنامه" });
            lineChart.Series = lineChartItems.OrderBy (x => x.DataNumber).ToList ();
            MonthlyData = lineChart;
        }

        private async Task<List<SelectListItem>> _FetchLinesAsSelectAsync () {
            var result = await _context
                .TblHall.Select (x => new {
                    Value = x.Id.ToString (),
                        Text = x.Line,
                        _HelperSort = x.HallType,
                }).OrderBy (x => x._HelperSort).ToListAsync ();
            return result.Select (x => new SelectListItem {
                Value = x.Value, Text = x.Text
            }).ToList ();
        }
    }
}
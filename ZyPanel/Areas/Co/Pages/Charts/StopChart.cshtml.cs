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
using DbLayer.Enums;
using ZyPanel.Helper.Vmodel;

namespace ZyPanel.Areas.Co.Pages.Charts {
    public class StopChartModel : PageModel {

        public readonly AppDbContext _context;

        public StopChartModel (AppDbContext context) { _context = context; }

        public LineChartVm ChartData { get; set; } = default;

        public int StartYear => 1400;
        public int CurrentYear => PersianCulture.GetPersianYear (DateTime.Now);

        [BindProperty (SupportsGet = true)]
        public HallType HallType { get; set; } = HallType.CastIron;

        [BindProperty (SupportsGet = true)]
        public int? Year { get; set; }

        public async Task<IActionResult> OnGetAsync () {
            Year = Year ?? this.CurrentYear;
            if (HallType != HallType.Default) {
                await fillChartAsync ();
            }
            return Page ();
        }

        private async Task fillChartAsync () {
            var lineChart = new LineChartVm ();
            var persianDate = PersianCulture.GetPersianYearStartAndEndDates (Year.Value);
            // 
            // Planning
            // 
            var planning = await _context.TblPlanning
                .Where (x => x.TblHall.HallType == HallType &&
                    x.BeginDate >= persianDate.StartDate &&
                    x.FinishDate <= persianDate.EndDate && x.TblStopInfo.Any ())
                .Include (x => x.TblHall).AsNoTracking ()
                .Select (x => new {
                    Id = x.Id,
                        LineId = x.TblHallId,
                        LineTitle = x.TblHall.Line,
                        HallType = x.TblHall.HallType,
                }).ToListAsync ();
            var planningIds = planning.Select (x => x.Id).ToArray ();
            var stopInfo = await _context.TblStopInfo
                .Where (x => planningIds.Any (y => y == x.TblPlanningId))
                .GroupBy (x => new { PlanningId = x.TblPlanningId, Month = x.ShamsiDate.Month })
                .Select (x => new {
                    PlanningId = x.Key.PlanningId,
                        MonthNumber = x.Key.Month,
                        Time = x.Sum (y => EF.Functions.DateDiffMinute (y.BeginDate, y.FinishDate.Value))
                }).ToListAsync ();
            // 
            // Calculate
            // 
            var lineChartItems = new List<LineChartItemVm> ();
            var distinctedPlanningIds = planningIds.Distinct ();
            var lineCount = planning.Select (x => x.LineId).Distinct ().Count ();
            for (var i = 1; i <= 12; i++) {
                var newItem = new LineChartItemVm ();
                var lineValue = new List<string> ();
                newItem.DataNumber = i;
                newItem.DataTitle = PersianCulture.GetPersianMonthName (i);
                foreach (var item in distinctedPlanningIds) {
                    var newData = stopInfo.FirstOrDefault (x => x.PlanningId == item && x.MonthNumber == i);
                    if (newData != null) {
                        lineValue.Add (Math.Abs (newData.Time).ToString ());
                    } else {
                        lineValue.Add ("0");
                    }
                }
                newItem.DataValue = string.Join (",", lineValue);
                lineChartItems.Add (newItem);
            }
            lineChart.Series = lineChartItems.OrderBy (x => x.DataNumber).ToList ();

            // 
            // Fill the series
            // 
            var lineIds = planning.OrderBy (x => x.Id)
                .Select (x => x.LineId).Distinct ();
            var timeTargets = await _context.TblHallSchedule
                .Where (x => lineIds.Any (y => y == x.TblHallId) && x.Year == Year.Value)
                .Select (x => new { LineId = x.TblHallId, Time = x.Interval }).ToListAsync ();

            var seriesTitle = new List<String> ();
            foreach (var item in lineIds) {
                var newItem = string.Empty;
                var target = timeTargets.FirstOrDefault (x => x.LineId == item);
                var time = target != null ? target.Time.TotalMinutes : 0;
                var timeFormat = time <= 60 ? $"{time}m" : $"{(int)time/60}h : {time%60}m";
                newItem = $"{planning.First (x => x.LineId == item).LineTitle} - {timeFormat}";
                seriesTitle.Add (newItem);
            }
            lineChart.SeriesTitle = string.Join (",", seriesTitle);
            ChartData = lineChart;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

// 
using CldLayer.Persian;
using DbLayer.Context;
using DbLayer.DbTable;
using DbLayer.Enums;
using HpLayer.Extensions;
using ZyPanel.Helper.Utils;
using ZyPanel.Helper.Vmodel;

namespace ZyPanel.Areas.Reports.Pages.Reports {
    public class MYReportModel : PageModel {
        private DateTime _startMonth;
        private static DateTime _today;
        public readonly AppDbContext _context;
        private static DateTime _now = DateTime.Now;
        private static DateTime _endOfToday = new DateTime (_now.Year, _now.Month, _now.Day, 23, 59, 59);

        public MYReportModel (AppDbContext context) {
            _context = context;
        }

        #region ### Model ###
        public class HallModel {
            public byte HallId { get; set; }

            public string HallTitle { get; set; }

            public List<LineModel> Lines { get; set; }
        }

        public class LineModel {
            public long LineId { get; set; }

            public string LineTitle { get; set; }

            public bool IsWorking { get; set; }
            public StopInfoItemVm MonthLy { get; set; }

            public StopInfoItemVm YearLy { get; set; }
        }

        #endregion

        #region ### List ###
        public List<HallModel> StopInfoList { get; set; }
        #endregion

        #region ### Vm ###
        public class StopInfoItemVm {
            public string Target { get; set; }
            public string Performance { get; set; }
            public string Tolerance { get; set; }
        }

        private class BeginFinishVm {
            public DateTime BeginDate { get; set; }

            public DateTime? FinishDate { get; set; }
        }
        #endregion

        public string Date { get; set; }

        public async Task<IActionResult> OnGetAsync (string date) {
            var hallData = new List<HallModel> ();
            Func<HallType, bool> enumCondition = x => x == HallType.Aluminium || x == HallType.CastIron;
            Expression<Func<TblHall, bool>> hallCondition = x => x.HallType == HallType.Aluminium || x.HallType == HallType.CastIron;
            // 
            // Calculate today
            // 
            _today = DateTime.Now;
            if (!string.IsNullOrEmpty (date)) {
                _today = date.ToGregorianDateTimeOrDefault ();
            }
            Date = _today.ToPersianDateTextify ();
            // 
            // Calculate the month
            // 
            var dayOfMonth = _today.GetPersianDayOfMonth ();
            _startMonth = _today.AddDays (-(dayOfMonth - 1));
            // 
            // Calculate the year
            // 
            var hijriYear = _today.GetPersianYear ();
            var aYearAgo = $"{hijriYear}/01/01".ToGregorianDateTimeOrDefault ();
            // 
            // Hall
            // 
            var hallsData = await _context.TblHall
                .Where (hallCondition)
                .Include (x => x.TblHallSchedule.Where (y => y.Year == hijriYear))
                .Select (x => new {
                    Id = x.Id,
                        Line = x.Line,
                        HallType = x.HallType,
                        IsWorking = x.IsWorking,
                        Interval = x.TblHallSchedule.Any () ?
                        x.TblHallSchedule.FirstOrDefault ().Interval : TimeSpan.Zero
                }).ToListAsync ();
            // 
            // StopInfo
            // 
            var lineIds = hallsData.Select (x => x.Id).ToArray ();
            var stopInfo = await _context.TblStopInfo
                .Include (x => x.TblPlanning)
                .Where (x => lineIds.Any (y => y == x.TblPlanning.TblHallId) &&
                    (_today.Date >= aYearAgo.Date && _today >= x.BeginDate))
                .Select (x => new {
                    BeginDate = x.BeginDate,
                        FinishDate = x.FinishDate,
                        CalculatedTime = x.CalculatedTime,
                        TblPlanningId = x.TblPlanningId,
                        TblHallId = x.TblPlanning.TblHallId
                }).ToListAsync ();
            // 
            // Production
            // 
            var productionInfo = await _context.TblProductionInfo
                .Where (x => lineIds.Any (y => y == x.TblPlanning.TblHallId) &&
                    (x.CreatedDate.Date >= aYearAgo.Date && x.CreatedDate.Date <= _today.Date))
                .AsNoTracking ().Select (x => new {
                    HallId = x.HallId,
                        PutCount = x.PutCount,
                        WeightCode = x.WeightCode,
                        CreatedDate = x.CreatedDate,
                        TblPlanningId = x.TblPlanningId
                }).ToListAsync ();
            // 
            // Final process
            // 
            var hallTypes = Enum.GetValues (typeof (HallType)).Cast<HallType> ().Where (enumCondition);
            foreach (var hallType in hallTypes) {
                var hallItem = new HallModel ();
                hallItem.HallId = (byte) hallType;
                hallItem.HallTitle = EnumExtensions.GetDisplayName ((HallType) hallType);
                var lineIdsByHall = hallsData.Where (x => x.HallType == hallType).Select (x => x.Id);
                var productionsByHall = productionInfo.Where (x => lineIdsByHall.Any (y => y == x.HallId)).ToList ();
                var planningIdsByHall = productionsByHall.Select (x => x.TblPlanningId).ToArray ();

                #region :: StopInfo ::
                var lineModel = new List<LineModel> ();
                foreach (var lineId in lineIdsByHall) {
                    var lineItem = new LineModel ();
                    var lineStop = stopInfo.Where (x => x.TblHallId == lineId)
                        .Select (x => new BeginFinishVm {
                            BeginDate = x.BeginDate, FinishDate = x.FinishDate
                        }).ToList ();
                    var productionDatesByLine = productionInfo
                        .Where (x => x.HallId == lineId).Select (x => x.CreatedDate).ToList ();
                    // 
                    // 
                    // 
                    // line
                    var line = hallsData.FirstOrDefault (x => x.Id == lineId);
                    lineItem.LineId = lineId;
                    lineItem.LineTitle = line.Line;
                    lineItem.IsWorking = line.IsWorking;
                    var targetPerDay = line.Interval.TotalMinutes;
                    // 
                    // monthly
                    // 
                    lineItem.MonthLy = getMonthStopInfoData (lineStop, productionDatesByLine, targetPerDay);
                    // 
                    // yearly
                    // 
                    lineItem.YearLy = getYearStopInfoData (lineStop, productionDatesByLine, targetPerDay);
                    // 
                    // 
                    // 
                    lineModel.Add (lineItem);
                }
                hallItem.Lines = lineModel;
                hallData.Add (hallItem);
                #endregion
            }
            StopInfoList = hallData;
            return Page ();
        }

        private StopInfoItemVm getStopInfoData (int target, int performance) {
            var result = new StopInfoItemVm ();
            // target
            result.Target = FormatHelper.GetTimeValue (target);
            // performance
            result.Performance = FormatHelper.GetTimeValue (performance);
            // tolerance
            int tolerance = default;
            if (target != 0) {
                tolerance = 100 - (performance * 100 / target);
            }
            result.Tolerance = FormatHelper.GetPercentFormat (tolerance);
            return result;
        }

        private int countDaysByTwoDates (List<BeginFinishVm> stopInfo, List<DateTime> productionDates) {
            int betweenDays = default;
            var beginDates = stopInfo.GroupBy (x => x.BeginDate.Date).Select (x => new { date = x.Key }).ToList ();
            var finishDates = stopInfo.GroupBy (x => x.FinishDate.Value.Date).Select (x => new { date = x.Key }).ToList ();
            var pDates = productionDates.GroupBy (x => x.Date).Select (x => new { date = x.Key }).ToList ();
            var allDaysCount = beginDates.Concat (finishDates).Concat (pDates).Select (x => new { x.date.Date }).Distinct ();
            foreach (var item in stopInfo) {
                if (item.BeginDate.Date != item.FinishDate.Value.Date) {
                    betweenDays += ((int) (item.FinishDate.Value.Date - item.BeginDate.Date).TotalDays) - 1;
                }
            }
            return betweenDays + allDaysCount.Count ();
        }

        // 
        private StopInfoItemVm getMonthStopInfoData (List<BeginFinishVm> data, List<DateTime> dates, double targetPerDay) {
            int performance = default;
            var result = data.Where (x => !x.FinishDate.HasValue)
                .Select (x => new BeginFinishVm {
                    BeginDate = x.BeginDate.Date < _startMonth.Date ? _startMonth : x.BeginDate, FinishDate = _endOfToday
                }).ToList ();
            var tempData = data.Where (x => x.FinishDate.HasValue)
                .Select (x => new BeginFinishVm {
                    BeginDate = x.BeginDate, FinishDate = x.FinishDate.Value
                }).ToList ();
            result.AddRange (
                tempData.Where (x => x.FinishDate.Value.Date >= _startMonth.Date)
                .Select (x => new BeginFinishVm {
                    BeginDate = x.BeginDate.Date > _startMonth.Date ? x.BeginDate : _startMonth,
                        FinishDate = x.FinishDate.Value.Date > _today.Date ? _endOfToday : x.FinishDate
                }).ToList ()
            );
            performance = (int) (result.Sum (x => (x.FinishDate.Value - x.BeginDate).TotalMinutes));
            var days = countDaysByTwoDates (result, dates);
            var target = (int) (days * targetPerDay);
            var finalResult = getStopInfoData (target, performance);
            return finalResult;
        }

        private StopInfoItemVm getYearStopInfoData (List<BeginFinishVm> data, List<DateTime> dates, double targetPerDay) {
            int performance = default;
            var result = data
                .Select (x => new BeginFinishVm {
                    BeginDate = x.BeginDate,
                        FinishDate = x.FinishDate.HasValue ? x.FinishDate : _endOfToday
                }).ToList ();
            performance = (int) (result.Sum (x => (x.FinishDate.Value - x.BeginDate).TotalMinutes));
            var days = countDaysByTwoDates (result, dates);
            var target = (int) (days * targetPerDay);
            var finalResult = getStopInfoData ((int) target, performance);
            return finalResult;
        }
    }
}
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

namespace ZyPanel.Areas.Reports {
    public class IndexModel : PageModel {
        private DateTime _startMonth;
        private static DateTime _today;
        public readonly AppDbContext _context;
        private static DateTime _now = DateTime.Now;
        private static DateTime _endOfToday = new DateTime (_now.Year, _now.Month, _now.Day, 23, 59, 59);
        private static DateTime _startFirstShift = new DateTime (_now.Year, _now.Month, _now.Day, 0, 0, 0);

        public IndexModel (AppDbContext context) {
            _context = context;
        }

        #region ### Model ###
        public class HallModel {
            public byte HallId { get; set; }

            public string HallTitle { get; set; }

            public List<LineModel> Lines { get; set; }
        }

        public class KarsetModel {
            public string KarsetTitle { get; set; }

            public KarsetItemVm Daily { get; set; }

            public KarsetItemVm Monthly { get; set; }

            public KarsetItemVm Yearly { get; set; }
        }

        public class LineModel {
            public long LineId { get; set; }

            public string LineTitle { get; set; }

            public bool IsWorking { get; set; }

            public StopInfoItemVm Daily { get; set; }

            public StopInfoItemVm MonthLy { get; set; }

            public StopInfoItemVm YearLy { get; set; }
        }

        public class GuageChartModel {
            public byte Id { get; set; }

            public string Hall { get; set; }

            public GuageItemVm Daily { get; set; }

            public GuageItemVm Monthly { get; set; }

            public GuageItemVm Yearly { get; set; }
        }

        public class LatestTemplatesModel {
            public long HallId { get; set; }

            public long LineId { get; set; }

            public string LineTitle { get; set; }

            public string ProductsTitle { get; set; }

            public string WeightCode { get; set; }
        }

        public class ProductsWeightModel {
            public long LineId { get; set; }

            public string ProductTitle { get; set; }

            public int Count { get; set; }

            public string Weight { get; set; }
        }

        public class LineChartModel {
            public byte HallType { get; set; }

            public LineChartVm YearlyData { get; set; }

            public LineChartVm WeeklyData { get; set; }
        }

        #endregion

        #region ### List ###
        public List<HallModel> StopInfoList { get; set; }

        public List<KarsetModel> KarsetList { get; set; }

        public List<LineChartModel> LineChartList { get; set; }

        public List<GuageChartModel> GuageChartList { get; set; }

        public List<LatestTemplatesModel> LatestTemplatesList { get; set; }

        public List<ProductsWeightModel> ProductsWeightList { get; set; }
        #endregion

        #region ### Vm ###
        private class WeightVm {
            public string WeightCode { get; set; }

            public double ProductWeightAsKg { get; set; }

            public double TemplateWeightAsKg { get; set; }
        }

        private class ProductionVm {
            public string WeightCode { get; set; }

            public double PutCount { get; set; }
        }

        public class StopInfoItemVm {
            public string Target { get; set; }
            public string Performance { get; set; }
            public string Tolerance { get; set; }
        }

        private class BeginFinishVm {
            public DateTime BeginDate { get; set; }

            public DateTime? FinishDate { get; set; }
        }

        public class GuageItemVm {
            public int Target { get; set; }
            public string Performance { get; set; }
            public string Offset { get; set; }
            public string OffsetAsString { get; set; }
        }

        public class KarsetItemVm {
            public string ProductTonnage { get; set; }

            public string PlanningTonnage { get; set; }

            public string Available { get; set; }
        }
        #endregion

        public string Date { get; set; }

        public async Task<IActionResult> OnGetAsync (string date) {
            var hallData = new List<HallModel> ();
            var guageData = new List<GuageChartModel> ();
            var lineData = new List<LineChartModel> ();
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
            await fillKarsetAsync (aYearAgo);
            await fillLatestTemplates ();
            await fillYesterdayProducts ();
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
            // Tonnage
            // 
            var planningIds = productionInfo.Select (x => x.TblPlanningId).ToArray ();
            var tonnage = await _context.TblTonnage.Where (x => planningIds.Any (y => y == x.TblPlanningId))
                .Select (x => new {
                    UsedTonnageAsKg = x.UsedTonnageAsKg,
                        TblPlanningId = x.TblPlanningId,
                        TonnageDate = x.TonnageDate,
                }).ToListAsync ();
            // 
            // Weight
            // 
            var weightCodes = productionInfo.Where (x => lineIds.Any (y => y == x.HallId)).Select (x => x.WeightCode);
            var weightes = await _context.JoinTP
                .Where (x => weightCodes.Any (y => y == x.WeightCode)).Select (x => new WeightVm {
                    WeightCode = x.WeightCode,
                        ProductWeightAsKg = x.ProductWeightAsKg,
                        TemplateWeightAsKg = x.TemplateWeightAsKg
                }).ToListAsync ();
            // 
            // Schedule
            // 
            var schedule = await _context
                .TblSchedule.FirstOrDefaultAsync (x => x.ScheduleType == ScheduleType.Single && x.SubsetId != null);
            if (schedule.BeginTime != null) {
                _startFirstShift = _startFirstShift.AddMinutes (schedule.BeginTime.Value.TotalMinutes);
            }
            // 
            // Final process
            // 
            var hallTypes = Enum.GetValues (typeof (HallType)).Cast<HallType> ().Where (enumCondition);
            foreach (var hallType in hallTypes) {
                var hallItem = new HallModel ();
                var guageItem = new GuageChartModel ();
                hallItem.HallId = guageItem.Id = (byte) hallType;
                hallItem.HallTitle = guageItem.Hall = EnumExtensions.GetDisplayName ((HallType) hallType);
                var lineIdsByHall = hallsData.Where (x => x.HallType == hallType).Select (x => x.Id);
                var productionsByHall = productionInfo.Where (x => lineIdsByHall.Any (y => y == x.HallId)).ToList ();
                var planningIdsByHall = productionsByHall.Select (x => x.TblPlanningId).ToArray ();

                #region :: Line chart ::
                var lineChart = new LineChartModel ();
                lineChart.HallType = (byte) hallType;
                lineChart.YearlyData = await getYearlyChartAsync (lineIdsByHall);
                lineChart.WeeklyData = await getWeeklyChartAsync (lineIdsByHall);
                lineData.Add (lineChart);
                #endregion

                #region :: Guage chart ::
                // 
                // Daily guage
                // 
                var gdailyTarget = tonnage.Where (x => planningIdsByHall
                    .Any (y => y == x.TblPlanningId) && x.TonnageDate.Date == _today.Date).Sum (x => x.UsedTonnageAsKg);
                var gdailyGuage = productionsByHall.Where (x => x.CreatedDate.Date == _today.Date)
                    .Select (x => new ProductionVm { PutCount = x.PutCount, WeightCode = x.WeightCode }).ToList ();
                guageItem.Daily = getGuageChartData (gdailyGuage, weightes, gdailyTarget);
                // 
                // Monthly guage
                // 
                var gmonthlyTarget = tonnage.Where (x => planningIdsByHall
                    .Any (y => y == x.TblPlanningId) && x.TonnageDate.Date >= _startMonth.Date && x.TonnageDate.Date <= _today.Date).Sum (x => x.UsedTonnageAsKg);
                var monthlyGuage = productionsByHall.Where (x => x.CreatedDate.Date >= _startMonth.Date && x.CreatedDate.Date <= _today.Date)
                    .Select (x => new ProductionVm { PutCount = x.PutCount, WeightCode = x.WeightCode }).ToList ();
                guageItem.Monthly = getGuageChartData (monthlyGuage, weightes, gmonthlyTarget);
                // 
                // Yearly guage
                // 
                var gyearlyTarget = tonnage.Where (x => planningIdsByHall
                    .Any (y => y == x.TblPlanningId) && x.TonnageDate.Date >= aYearAgo.Date).Sum (x => x.UsedTonnageAsKg);
                var yearlyGuage = productionsByHall.Where (x => x.CreatedDate.Date >= aYearAgo.Date)
                    .Select (x => new ProductionVm { PutCount = x.PutCount, WeightCode = x.WeightCode }).ToList ();
                guageItem.Yearly = getGuageChartData (yearlyGuage, weightes, gyearlyTarget);
                guageData.Add (guageItem);
                #endregion

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
                    // daily
                    // 
                    lineItem.Daily = getTodayStopInfoData (lineStop, productionDatesByLine, targetPerDay);
                    // 
                    // monthly
                    // 

                    // lineItem.MonthLy = getMonthData (lineStop, productionDatesByLine, targetPerDay);
                    // 
                    // yearly
                    // 
                    // lineItem.YearLy = getYearStopInfoData (lineStop, productionDatesByLine, targetPerDay);
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
            GuageChartList = guageData;
            LineChartList = lineData;
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

        private GuageItemVm getGuageChartData
            (List<ProductionVm> productions, List<WeightVm> weights, int targetInkg) {
                double performance = default;
                var result = new GuageItemVm ();
                foreach (var code in productions) {
                    var weight = weights.Where (x => x.WeightCode == code.WeightCode).ToList ();
                    var tweight = weight.FirstOrDefault ().TemplateWeightAsKg;
                    performance += code.PutCount * tweight;
                }
                if (targetInkg > 0) {
                    result.Target = targetInkg / 1000;
                    result.Offset = (performance * 100 / targetInkg).ToString ("N2");
                }
                result.Performance = performance > 0 ? (performance / 1000).ToString ("N2") : "0";
                result.OffsetAsString = result.Offset;
                return result;
            }

        private async Task fillLatestTemplates () {
            var result = new List<LatestTemplatesModel> ();
            var productionInfo = _context.TblProductionInfo
                .Where (x => x.CreatedDate.Date == _today.Date)
                .AsEnumerable ().GroupBy (x => x.HallId).Select (x => new {
                    HallId = x.Key,
                        ProductionId = x.Max (x => x.Id),
                        WeightCode = x.OrderByDescending (x => x.Id)
                        .FirstOrDefault ().WeightCode
                }).ToList ();
            var lineIds = productionInfo.Select (x => x.HallId).Distinct ();
            var weightCodes = productionInfo.Select (x => x.WeightCode).ToList ();
            var joinTp = await _context.JoinTP
                .Where (x => weightCodes.Any (y => y == x.WeightCode))
                .Include (x => x.TblProduct)
                .Select (x => new {
                    WeightCode = x.WeightCode,
                        productTitle = x.TblProduct.Title
                }).ToListAsync ();
            //
            //
            //
            var hallsData = await _context.TblHall
                .Where (x => lineIds.Any (y => y == x.Id)).Select (x => new {
                    Id = x.Id,
                        HallId = (byte) x.HallType,
                        LineTitle = x.Line,
                }).ToListAsync ();
            //
            //
            //
            foreach (var item in productionInfo) {
                var newItem = new LatestTemplatesModel ();
                newItem.HallId = hallsData.FirstOrDefault (x => x.Id == item.HallId).HallId;
                newItem.LineId = hallsData.FirstOrDefault (x => x.Id == item.HallId).Id;
                newItem.LineTitle = hallsData.FirstOrDefault (x => x.Id == item.HallId).LineTitle;
                var products = joinTp.Where (x => x.WeightCode == item.WeightCode)
                    .Select (x => x.productTitle).ToList ();
                newItem.ProductsTitle = string.Join ("-", products);
                newItem.WeightCode = joinTp
                    .FirstOrDefault (x => x.WeightCode == item.WeightCode).WeightCode;
                result.Add (newItem);
            }
            LatestTemplatesList = result;
        }

        private async Task fillYesterdayProducts () {
            var yesterday = DateTime.Now.AddDays (-1);
            var result = new List<ProductsWeightModel> ();
            var productionInfo = _context.TblProductionInfo
                .Where (x => x.CreatedDate.Date == yesterday.Date)
                .AsEnumerable ().GroupBy (x => x.WeightCode)
                .Select (x => new {
                    WeightCode = x.Key,
                        HallId = x.FirstOrDefault ().HallId,
                        PutCounts = x.Sum (x => x.PutCount),
                }).ToList ();
            var weightCodes = productionInfo.Select (x => x.WeightCode).ToList ();
            var joinTp = await _context.JoinTP
                .Where (x => weightCodes.Any (y => y == x.WeightCode))
                .Include (x => x.TblProduct)
                .Select (x => new {
                    WeightCode = x.WeightCode,
                        productTitle = x.TblProduct.Title,
                        ProductWeightAsKg = x.ProductWeightAsKg,
                }).ToListAsync ();
            var hallIds = productionInfo.Select (x => x.HallId).Distinct ().ToList ();

            foreach (var item in productionInfo) {
                var newItem = new ProductsWeightModel ();
                newItem.LineId = item.HallId;
                var putCount = item.PutCounts;
                newItem.Count = item.PutCounts;
                newItem.ProductTitle = joinTp.FirstOrDefault (x => x.WeightCode == item.WeightCode).productTitle;
                newItem.Weight = (putCount * joinTp.Where (x => x.WeightCode == item.WeightCode).Sum (x => x.ProductWeightAsKg)).ToString ("N2");
                result.Add (newItem);
            }
            ProductsWeightList = result;
        }

        // 
        private StopInfoItemVm getTodayStopInfoData (List<BeginFinishVm> data, List<DateTime> dates, double targetPerDay) {
            int performance = default;
            var result = data.Where (x => !x.FinishDate.HasValue)
                .Select (x => new BeginFinishVm {
                    BeginDate = x.BeginDate.Date == _today.Date ? x.BeginDate : _startFirstShift,
                        FinishDate = DateTime.Now
                }).ToList ();
            var tempData = data.Where (x => x.FinishDate.HasValue)
                .Select (x => new BeginFinishVm {
                    BeginDate = x.BeginDate,
                        FinishDate = x.FinishDate
                }).ToList ();
            result.AddRange (
                tempData.Where (x => x.FinishDate.Value.Date >= _today.Date)
                .Select (x => new BeginFinishVm {
                    BeginDate = x.BeginDate.Date == _today.Date ? x.BeginDate : _startFirstShift,
                        FinishDate = x.FinishDate.Value.Date > _today.Date ? _endOfToday : x.FinishDate
                }).ToList ()
            );
            performance = (int) (result.Sum (x => (x.FinishDate.Value - x.BeginDate).TotalMinutes));
            var finalResult = getStopInfoData ((int) targetPerDay, performance);
            return finalResult;
        }

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

        private async Task<LineChartVm> getYearlyChartAsync (IEnumerable<long> lineIds) {
            var lineChart = new LineChartVm ();

            var year = _today.GetPersianYear ();
            var persianDate = PersianCulture.GetPersianYearStartAndEndDates (year);
            // 
            // Planning
            // 
            var planningIds = await _context.TblPlanning
                .Where (x => lineIds.Any (y => y == x.TblHallId) &&
                    x.BeginDate >= persianDate.StartDate && x.FinishDate <= persianDate.EndDate)
                .Select (x => x.Id).ToListAsync ();
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
            lineChart.SeriesTitle = string.Join (',', new string[] { "برنامه", "تولید" });
            lineChart.Series = lineChartItems.OrderBy (x => x.DataNumber).ToList ();
            return lineChart;
        }

        private async Task<LineChartVm> getWeeklyChartAsync (IEnumerable<long> lineIds) {
            var lineChart = new LineChartVm ();
            var startAndEndWeek = StaticHelper.GetStartAndEndWeek (_today);
            var startWeek = startAndEndWeek.Item1;
            var finishWeek = startAndEndWeek.Item2;

            //
            //
            //
            var Dates = Enumerable.Range (0, 1 + finishWeek.Subtract (startWeek).Days)
                .Select (offset => startWeek.AddDays (offset))
                .ToArray ();
            // 
            // Planning
            // 
            var planningIds = await _context.TblPlanning
                .Where (x => lineIds.Any (y => y == x.TblHallId) && x.BeginDate.Date >= startWeek.Date &&
                    x.FinishDate.Date <= finishWeek.Date).Select (x => x.Id).ToListAsync ();
            // 
            // Tonnage
            // 
            var tonnage = await _context.TblTonnage
                .Where (x => planningIds.Any (y => y == x.TblPlanningId))
                .GroupBy (x => x.ShamsiDate.Day).Select (x => new {
                    Day = x.Key,
                        WeightAsKg = x.Sum (k => k.UsedTonnageAsKg)
                }).ToListAsync ();
            // 
            // Production / Template
            // 
            var production = await _context.TblProductionInfo
                .Where (x => planningIds.Any (y => y == x.TblPlanningId))
                .GroupBy (x => new { x.ShamsiDate.Day, x.WeightCode }).Select (x => new {
                    Day = x.Key.Day,
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
            for (var i = 0; i < 7; i++) {
                var newItem = new LineChartItemVm ();
                newItem.DataNumber = i + 1;
                double productValue = default;
                var lineValues = new List<string> ();
                var dayOfWeek = i == 0 ? 6 : i - 1;
                newItem.DataTitle = PersianCulture.GetPersianWeekDayName ((DayOfWeek) dayOfWeek);

                var dateByDayOfWeek = Dates
                    .Where (x => x.DayOfWeek == (DayOfWeek) dayOfWeek).FirstOrDefault ();
                var dayOfMonth = dateByDayOfWeek.GetPersianDayOfMonth ();
                lineValues.Add (tonnage.Where (x => x.Day == dayOfMonth).Sum (x => x.WeightAsKg).ToString ());
                foreach (var item in production.Where (x => x.Day == dayOfMonth).ToList ()) {
                    var code = item.WeightCode;
                    var templateCount = item.TemplateCounts;
                    var templateWeight = joinTp.FirstOrDefault (x => x.WeightCode == code).TemplateWeightAsKg;
                    productValue += (double) templateCount * templateWeight;
                }
                lineValues.Add (productValue.ToString ());
                newItem.DataValue = string.Join (",", lineValues);
                lineChartItems.Add (newItem);
            }
            lineChart.SeriesTitle = string.Join (',', new string[] { "برنامه", "تولید" });
            lineChart.Series = lineChartItems.OrderBy (x => x.DataNumber).ToList ();
            return lineChart;
        }

        private async Task fillKarsetAsync (DateTime startYear) {
            var result = new List<KarsetModel> ();
            var yesterday = _today.AddDays (-1);
            var karsetResult = await _context.TblKarset
                .Where (x => x.CreateDate.Date >= startYear).Select (x => new {
                    Id = x.Id,
                        Type = x.KarsetType,
                        CreateDate = x.CreateDate,
                        ProductTonnage = x.ProductTonnage,
                        PlanningTonnage = x.PlanningTonnage,
                        Available = x.Available,
                }).ToListAsync ();
            foreach (var item in Enum.GetValues (typeof (KarsetType)).Cast<KarsetType> ()) {
                var karsetModel = new KarsetModel ();
                var karsetData = karsetResult.Where (x => x.Type == item).ToList ();
                karsetModel.KarsetTitle = EnumExtensions.GetDisplayName (item);
                // daily
                var dailyKarset = new KarsetItemVm ();
                var dailyProductTonnage = karsetData.Where (x => x.CreateDate.Date == yesterday.Date).Sum (x => x.ProductTonnage);
                var dailyPlanningTonnage = karsetData.Where (x => x.CreateDate.Date == yesterday.Date).Sum (x => x.PlanningTonnage);

                dailyKarset.ProductTonnage = (dailyProductTonnage / 1000).ToString ("N2");
                dailyKarset.PlanningTonnage = (dailyPlanningTonnage / 1000).ToString ("N2");

                dailyKarset.Available = dailyPlanningTonnage > 0 ? $"{(dailyProductTonnage * 100 / dailyPlanningTonnage).ToString("N2")} %" : "0";
                karsetModel.Daily = dailyKarset;

                // monthly 
                var monthlyKarset = new KarsetItemVm ();
                var monthlyProductTonnage = karsetData.Where (x => x.CreateDate.Date >= _startMonth.Date).Sum (x => x.ProductTonnage);
                var monthlyPlanningTonnage = karsetData.Where (x => x.CreateDate.Date >= _startMonth.Date).Sum (x => x.PlanningTonnage);

                monthlyKarset.ProductTonnage = (monthlyProductTonnage / 1000).ToString ("N2");
                monthlyKarset.PlanningTonnage = (monthlyPlanningTonnage / 1000).ToString ("N2");
                monthlyKarset.Available = monthlyPlanningTonnage > 0 ? $"{(monthlyProductTonnage * 100 / monthlyPlanningTonnage).ToString("N2")} %" : "0";
                karsetModel.Monthly = monthlyKarset;

                // yearly  
                var yearlyKarset = new KarsetItemVm ();
                var yearlyProductTonnage = karsetData.Sum (x => x.ProductTonnage);
                var yearlyPlanningTonnage = karsetData.Sum (x => x.PlanningTonnage);

                yearlyKarset.ProductTonnage = (yearlyProductTonnage / 1000).ToString ("N2");
                yearlyKarset.PlanningTonnage = (yearlyPlanningTonnage / 1000).ToString ("N2");
                yearlyKarset.Available = yearlyPlanningTonnage > 0 ? $"{(yearlyProductTonnage * 100 / yearlyPlanningTonnage).ToString("N2")} %" : "0";
                karsetModel.Yearly = yearlyKarset;
                // 
                result.Add (karsetModel);
            }
            KarsetList = result;
        }
    }
}
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

// 
using CldLayer.Persian;
using DbLayer.Context;
using DbLayer.DbTable;
using Microsoft.AspNetCore.Mvc;
using ZyPanel.Areas.Shared;
using ZyPanel.Helper.Utils;

namespace ZyPanel.Areas.Reports.Pages.Temp {
    public class QControlDetailsModel : FetchRootPage<TblQControl> {

        public QControlDetailsModel (AppDbContext context) : base (context) { }

        [BindProperty (SupportsGet = true)]
        public long ProductId { get; set; }

        [BindProperty (SupportsGet = true)]
        public string BeginDate { get; set; }

        [BindProperty (SupportsGet = true)]
        public string FinishDate { get; set; }

        public string Hall { get; set; }

        public string Product { get; set; }

        public class ListModel {
            public long Id { get; set; }

            public string CreateDate { get; set; }

            public string ProductionDate { get; set; }

            public string DefectTitle { get; set; }

            public string DefectPlaceTitle { get; set; }

            // 
            public int Waste { get; set; }

            public int Healthy { get; set; }

            public double WastePercent { get; set; }

            public int BackCount { get; set; } = 0;

            public string BackTitle => BackCount == 0 ? "تولیدی" : "مشتری";

            public bool IsWaste { get; set; }

            // 
            public string StationTitle { get; set; }

            public string TrackCode { get; set; }

            public string ShieldType { get; set; }

            public string Inspecter { get; set; }

            public string Explanation { get; set; }
        }

        public PaginatedList<ListModel> List { get; set; }

        public async Task OnGetAsync (int p = 1) {
            // var beginDate = BeginDate.ToGregorianDateTimeOrDefault ();
            // var finishDate = FinishDate.ToGregorianDateTimeOrDefault ();
            // List = await PaginatedList<ListModel>.CreateAsync (_dbSet
            //     .Where (x => x.ProductionDate.Date >= beginDate.Date &&
            //         x.ProductionDate <= finishDate.Date &&
            //         x.TblProductId == ProductId).Include (x => x.TblDefect)
            //     .Select (x => new ListModel {
            //         Id = x.Id,
            //             CreateDate = x.PersianCreatedDate,
            //             ProductionDate = x.PersianProductionDate,
            //             DefectTitle = x.TblDefect.Title,
            //             DefectPlaceTitle = x.DefectPlaceTitle,
            //             Waste = x.Waste,
            //             Healthy = x.Healthy,
            //             WastePercent = (double) (x.Waste * 100) / x.Healthy,
            //             BackCount = x.BackCount,
            //             IsWaste = x.IsWaste,
            //             StationTitle = x.StationTitle,
            //             TrackCode = x.TrackCode,
            //             ShieldType = x.ShieldType,
            //             Inspecter = x.Inspecter,
            //             Explanation = x.Explanation
            //     }).OrderByDescending (x => x.Id), p, _pageSize
            // );
            // // 
            // var qControl = await _dbSet.Include (x => x.TblProduct)
            //     .FirstOrDefaultAsync (x => x.TblProductId == ProductId);
            // Product = qControl.TblProduct.Title;
            // var hall = await _context.TblHall
            //     .FirstOrDefaultAsync (x => x.Id == qControl.TblHallId);
            // Hall = hall.FullTitle;
        }
    }
}
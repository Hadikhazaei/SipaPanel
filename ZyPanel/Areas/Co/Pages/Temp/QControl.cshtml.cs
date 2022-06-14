using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

// 
using CldLayer.Persian;
using DbLayer.Context;
using DbLayer.DbTable;
using ZyPanel.Areas.Shared;
using ZyPanel.Helper.Utils;

namespace ZyPanel.Areas.Co.Pages.Temp {
    public class QControlModel : FetchRootPage<TblQControl> {

        public QControlModel (AppDbContext context) : base (context) { }

        public string BeginDate { get; set; }

        public string FinishDate { get; set; }

        public class Filter {
            public int P { get; set; } = 1;

            public string BeginDate { get; set; }

            public string FinishDate { get; set; }
        }

        public class ListModel {
            public long HallId { get; set; }
            public string Hall { get; set; }

            public long ProductId { get; set; }

            public string Product { get; set; }

            public int Waste { get; set; }

            public int Healthy { get; set; }

            public double WastePercent { get; set; }
        }

        public PaginatedList<ListModel> List { get; set; }

        public async Task OnGetAsync (Filter filter) {
            // if (string.IsNullOrEmpty (filter.BeginDate)) {
            //     filter.BeginDate = DateTime.Now.ToShortPersianDateString ();
            // }
            // if (string.IsNullOrEmpty (filter.FinishDate)) {
            //     filter.FinishDate = DateTime.Now.ToShortPersianDateString ();
            // }
            // BeginDate = filter.BeginDate;
            // FinishDate = filter.FinishDate;
            // var beginDate = filter.BeginDate.ToGregorianDateTimeOrDefault ();
            // var finishDate = filter.FinishDate.ToGregorianDateTimeOrDefault ();
            // List = await PaginatedList<ListModel>.CreateAsync (_dbSet
            //     .Where (x => x.ProductionDate.Date >= beginDate.Date && x.ProductionDate.Date <= finishDate.Date)
            //     .GroupBy (x => new { x.TblHallId, x.TblProductId }).Select (x => new ListModel {
            //         HallId = x.Key.TblHallId,
            //             ProductId = x.Key.TblProductId,
            //             Waste = x.Sum (x => x.Waste),
            //             Healthy = x.Sum (x => x.Healthy),
            //             WastePercent = (double) (x.Sum (x => x.Waste) * 100) / x.Sum (x => x.Healthy)
            //     }).OrderBy (x => x.HallId), filter.P, _pageSize
            // );
            // // 
            // // fetch related halls
            // // 
            // var hallsId = List.Select (x => x.HallId).ToArray ();
            // var halls = await _context.TblHall.Where (x => hallsId.Any (p => p == x.Id))
            //     .Select (x => new { Id = x.Id, Title = x.FullTitle, }).ToListAsync ();
            // // 
            // // fetch related products
            // // 
            // var productsId = List.Select (x => x.ProductId).ToArray ();
            // var products = await _context.TblProduct.Where (x => productsId.Any (p => p == x.Id))
            //     .Select (x => new { Id = x.Id, Title = x.Title, }).ToListAsync ();
            // // 
            // // compelet the final list
            // // 
            // foreach (var item in List) {
            //     item.Hall = halls.First (x => x.Id == item.HallId).Title;
            //     item.Product = products.First (x => x.Id == item.ProductId).Title;
            // }
        }
    }
}
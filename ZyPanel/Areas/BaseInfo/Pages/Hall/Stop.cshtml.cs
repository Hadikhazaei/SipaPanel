using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// 
using DbLayer.Context;
using DbLayer.DbTable;
using ZyPanel.Areas.Shared;
using ZyPanel.Helper.Utils;
using ZyPanel.Helper.Vmodel;

namespace ZyPanel.Areas.BaseInfo.Pages.Hall {
    public class StopModel : FetchRootPage<TblStop> {
        public StopModel (AppDbContext context) : base (context) { }

        [BindProperty (SupportsGet = true)]
        public long Id { get; set; }

        public string HallInfo { get; set; }

        public class ListModel {
            public string Code { get; set; }

            public string Title { get; set; }

            public string Brief { get; set; }
        }

        public PaginatedList<ListModel> List { get; set; }

        public async Task OnGetAsync ([FromQuery] FilterQs vm) {
            var hallInfo = await _context.TblHall.FirstOrDefaultAsync (x => x.Id == Id);
            HallInfo = $@"سالن : {hallInfo.FullTitle}";

            var entity = _dbSet.Where (x => x.TblHallId == Id).AsNoTracking ();
            if (!string.IsNullOrEmpty (vm.Filter)) {
                entity = _dbSet.Where (x => x.Code.Contains (vm.Filter));
            }
            List = await PaginatedList<ListModel>.CreateAsync (
                entity.OrderByDescending (x => x.Id)
                .Select (x => new ListModel {
                    Code = x.Code,
                        Title = x.Title,
                        Brief = x.Brief
                }), vm.P, 15
            );
        }
    }
}
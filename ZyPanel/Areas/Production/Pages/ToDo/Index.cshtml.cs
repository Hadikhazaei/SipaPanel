using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

// 
using DbLayer.Context;
using DbLayer.DbTable;
using DbLayer.DbTable.Identity;
using DbLayer.Enums;
using ZyPanel.Areas.Shared;
using ZyPanel.Helper.Utils;
using ZyPanel.Service.IdentityService;

namespace ZyPanel.Areas.Production.Pages.ToDo {
    public class IndexModel : ModifyRootPage<TblPlanning> {
        private readonly UserManager<AppUser> _userManager;
        public IndexModel (AppDbContext context, UserManager<AppUser> userManager) : base (context) {
            _userManager = userManager;
        }

        public class ListModel {
            public long Id { get; set; }
            public string Title { get; set; }
            public string HallTitle { get; set; }
            public short ConsideredDays { get; set; }
            public string BeginDate { get; set; }
            public string FinishDate { get; set; }
            public string CreatedDate { get; set; }
            public string ScheduleTitle { get; set; }
            public bool CanProducing { get; set; }
        }

        public PaginatedList<ListModel> List { get; set; }

        public async Task OnGetAsync (int p = 1) {
            var username = HttpContext.User.Identity.GetUserName ();;
            var user = await _userManager.FindByNameAsync (username);
            var isCo = await _userManager.IsInRoleAsync (user, nameof (RoleType.CoRole));
            var isPManager = await _userManager.IsInRoleAsync (user, nameof (RoleType.ProductionManagerRole));
            Expression<Func<TblPlanning, bool>> condition = x => isCo || isPManager ? true : x.TblHallId == user.TblHallId;
            var entity = _dbSet
                .Where (condition).AsNoTracking ()
                .Include (x => x.TblHall).Include (x => x.TblProductionInfo)
                .OrderByDescending (x => x.Id).AsSplitQuery ();
            List = await PaginatedList<ListModel>.CreateAsync (entity
                .Select (x => new ListModel {
                    Id = x.Id,
                        Title = x.Title,
                        HallTitle = x.TblHall.FullTitle,
                        ConsideredDays = x.ConsideredDays,
                        BeginDate = x.PersianBeginDate,
                        FinishDate = x.PFinishDate,
                        CreatedDate = x.PersianCreatedDate,
                        ScheduleTitle = x.ScheduleTitle,
                        CanProducing = DateTime.Now.Date >= x.BeginDate.Date && DateTime.Now.Date <= x.FinishDate.Date
                }), p, _pageSize
            );
        }
    }
}
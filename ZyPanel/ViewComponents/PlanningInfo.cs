using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// 
using DbLayer.Context;
using ZyPanel.Helper.Vmodel;

namespace ZyPanel.ViewComponents {
    public class PlanningInfoViewComponent : ViewComponent {
        private readonly AppDbContext _context;

        public PlanningInfoViewComponent (AppDbContext context) { _context = context; }

        public async Task<IViewComponentResult> InvokeAsync (long pid) {
            var planning = await _context.TblPlanning
                .Include (x => x.TblHall).FirstOrDefaultAsync (x => x.Id == pid);
            var planningInfo = new PlanningInfoVm ();
            planningInfo.Info = $@"سالن : {planning.TblHall.FullTitle} 
            ، عنوان برنامه : {planning.Title} ، تاریخ : {planning.PersianBeginDate} تا {planning.PFinishDate}";
            return View (planningInfo);
        }
    }
}
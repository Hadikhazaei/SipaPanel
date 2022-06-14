using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

// 
using DbLayer.Context;
using DbLayer.DbTable;
using DbLayer.Enums;
using ZyPanel.Areas.Shared;
using ZyPanel.Helper.Utils;
using ZyPanel.Helper.Vmodel;

namespace ZyPanel.Areas.Co.Pages.Template {
    public class AssignLineModel : ModifyRootPage<TblHallTemplate> {

        public AssignLineModel (AppDbContext context) : base (context) { }

        public class InputModel {
            public long TblTemplateId { get; set; }

            [Display (Name = "سالن : ")]
            [Required (ErrorMessage = ConstValues.RqError)]
            [Range (1, long.MaxValue, ErrorMessage = ConstValues.RgError)]
            public long TblHallId { get; set; }

            public List<SelectListItem> Halls { get; set; }
        }

        [BindProperty (SupportsGet = true)]
        public long Id { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class ListModel {
            public long Id { get; set; }

            public string HallTitle { get; set; }

            public string TemplateCode { get; set; }
        }

        public List<ListModel> List { get; set; }

        public async Task OnGetAsync ([FromQuery] FilterQs vm) {
            List = await _dbSet
                .Include (x => x.TblHall)
                .Include (x => x.TblTemplate)
                .Where (x => x.TblTemplateId == Id)
                .Select (x => new ListModel {
                    Id = x.TblHallId,
                        HallTitle = x.TblHall.FullTitle,
                        TemplateCode = x.TblTemplate.Code
                }).ToListAsync ();
        }

        public async Task<IActionResult> OnPostAsync () {
            if (ModelState.IsValid) {
                try {
                    var template = await _dbSet.Include (x => x.TblHall)
                        .FirstOrDefaultAsync (x => x.TblTemplateId == Id);
                    if (template?.TblHall.HallType == HallType.CastIron) {
                        throw new Exception ($"برای سالن چدن بیشتر از یک خط نمی توانید تعریف کنید.");
                    }
                    Input.TblTemplateId = Id;
                    await AddItemExtend<InputModel> (Input);
                } catch (Exception ex) {
                    ModelState.AddModelError ("", ex.Message);
                    Alert = ModelState.ModelStateAsError ();
                }
            }
            return RedirectToPage ("./AssignLine");
        }

        // handler
        public async Task<PartialViewResult> OnGetCreate () {
            var selectedList = await _FetchHallsAsSelectAsync ();
            return Partial ("_AssignLineCreate", new InputModel {
                Halls = selectedList
            });
        }

        // public async Task<IActionResult> OnPostRemove (long hallId) {
        //     var item = await _dbSet
        //         .FirstOrDefaultAsync (x => x.TblTemplateId == Id && x.TblHallId == hallId);
        //     await base.RemoveItem (item);
        //     Alert = ModelStateType.A200.ModelStateAsText ();
        //     return RedirectToPage ("./AssignLine");
        // }

        // private
        private async Task<List<SelectListItem>> _FetchHallsAsSelectAsync () {
            var result = await _context.TblHall
                .Include (x => x.TblHallTemplate)
                .Where (x => !x.TblHallTemplate.Any (x => x.TblTemplateId == Id))
                .Select (x => new {
                    Value = x.Id.ToString (),
                        Text = x.FullTitle,
                        _HelperSort = x.HallType
                }).OrderBy (x => x._HelperSort).ToListAsync ();
            var selectedList = result
                .Select (x => new SelectListItem {
                    Value = x.Value, Text = x.Text
                }).ToList ();
            selectedList.Insert (0, new SelectListItem () {
                Value = "0",
                    Text = "انتخاب کنید"
            });
            return selectedList;
        }
    }
}
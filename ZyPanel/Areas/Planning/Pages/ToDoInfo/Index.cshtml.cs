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
using ZyPanel.Areas.Shared;
using ZyPanel.Helper.Utils;

namespace ZyPanel.Areas.Planning.Pages.ToDoInfo {
    public class IndexModel : FetchRootPage<TblPlanningInfo> {
        public IndexModel (AppDbContext context) : base (context) { }

        [BindProperty (SupportsGet = true)]
        public long Id { get; set; }

        public class InputModel {
            [Display (Name = "قالب : ")]
            [Required (ErrorMessage = ConstValues.RqError)]
            [Range (1, long.MaxValue, ErrorMessage = ConstValues.RgError)]
            public long TblTemplateId { get; set; }

            [Display (Name = "تعداد قالب : ")]
            [Required (ErrorMessage = ConstValues.RqError)]
            [Range (1, int.MaxValue, ErrorMessage = ConstValues.RgError)]
            public int TemplateCount { get; set; } = 1;

            [Display (Name = "اولویت : ")]
            [Required (ErrorMessage = ConstValues.RqError)]
            [Range (1, int.MaxValue, ErrorMessage = ConstValues.RgError)]
            public short Order { get; set; }

            [Display (Name = "توضیحات : ")]
            public string Explanation { get; set; }

            public long TblPlanningId { get; set; }

            // helper
            public string WeightCode { get; set; }

            public List<SelectListItem> Templates { get; set; }

            public bool EditMode { get; set; } = false;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class ListModel {
            public long Id { get; set; }
            public string TemplateCode { get; set; }
            public string WeightCode { get; set; }
            public int TemplateCount { get; set; }
            public string TemplateWeightAsKg { get; set; }
            public double TotalTemplateWeightAsKg { get; set; }
            public byte KaviteCount { get; set; }
            public short Order { get; set; }
            public string CreatedDate { get; set; }
            public string Explanation { get; set; }
            public List<Products> Products { get; set; }
        }

        public List<ListModel> List { get; set; }

        public async Task OnGetAsync () {
            List = await _dbSet
                .Where (x => x.TblPlanningId == Id)
                .Include (x => x.TblTemplate)
                .ThenInclude (x => x.JoinTP)
                .ThenInclude (x => x.TblProduct)
                .Select (x => new ListModel {
                    Id = x.Id,
                        TemplateCode = x.TblTemplate.Code,
                        KaviteCount = x.TblTemplate.KaviteCount,
                        // 
                        TemplateWeightAsKg = x.TblTemplate.JoinTP
                        .FirstOrDefault (j => j.TblTemplateId == x.TblTemplateId && j.WeightCode == x.WeightCode).TemplateDisplayAsKg,
                        TotalTemplateWeightAsKg = ((double) x.TblTemplate.JoinTP
                            .FirstOrDefault (j => j.TblTemplateId == x.TblTemplateId && j.WeightCode == x.WeightCode).TemplateWeight * x.TemplateCount / 1000),
                        // 
                        Order = x.Order,
                        TemplateCount = x.TemplateCount,
                        WeightCode = x.WeightCode,
                        CreatedDate = x.PersianCreatedDate,
                        Explanation = x.Explanation,
                        Products = x.TblTemplate.JoinTP
                        .Where (j => j.TblTemplateId == x.TblTemplateId && j.WeightCode == x.WeightCode)
                        .Select (p => new Products {
                            ProductId = p.TblProductId,
                                ProductTitle = p.TblProduct.Title,
                                ProductCount = p.ProductCount,
                                ProductWeightAsKg = p.ProductDisplayAsKg,
                                TotalProductWeightAsKg = ((double) p.ProductWeightAsKg * x.TemplateCount),
                        }).ToList ()
                }).OrderByDescending (x => x.Id).ToListAsync ();
        }

        public async Task<IActionResult> OnPostAsync () {
            if (ModelState.IsValid) {
                try {
                    if (EditKey != null) {
                        await EditItemExtend (x => x.TblTemplateId,
                            x => x.TemplateCount, x => x.Order, x => x.Explanation);
                        await EditItemExtend (x => x.TemplateCount, x => x.Order, x => x.Explanation);
                        return RedirectToPage ("./Index");
                    } else {
                        var template = await _context.JoinTP
                            .FirstOrDefaultAsync (x => x.TblTemplateId == Input.TblTemplateId && x.IsReady);
                        Input.TblPlanningId = Id;
                        Input.WeightCode = template.WeightCode;
                        await AddItemExtend<InputModel> (Input);
                    }
                } catch (Exception ex) {
                    ModelState.AddModelError ("", ex.Message);
                    Alert = ModelState.ModelStateAsError ();
                }
            }
            return RedirectToPage ("./Index");
        }

        // handler
        public async Task<PartialViewResult> OnGetCreate () {
            var model = new InputModel ();
            var planning = await _context.TblPlanning.FindAsync (Id);
            model.Templates = await _context.TblTemplate
                .Include (x => x.JoinTP.Where (x => x.IsReady))
                .Include (x => x.TblPlanningInfo.Where (x => x.TblPlanningId == Id))
                .Where (x => x.IsActive && x.JoinTP.Any (x => x.IsReady) &&
                    !x.TblPlanningInfo.Any (x => x.TblPlanningId == Id) &&
                    x.TblHallTemplate.Any (x => x.TblHallId == planning.TblHallId))
                .Select (x => new SelectListItem {
                    Value = x.Id.ToString (), Text = x.Code
                }).ToListAsync ();
            model.Templates.Insert (0, new SelectListItem () { Value = "0", Text = "انتخاب کنید" });
            return Partial ("_Create", model);
        }

        public async Task<IActionResult> OnPostRemove (long pinfoid) {
            var planningIfo = await FindAsNotTrackedAsync (pinfoid);
            var hasProduction = await _context.TblProductionInfo
                .AnyAsync (x => x.TblPlanningId == planningIfo.TblPlanningId &&
                    x.TblTemplateId == planningIfo.TblTemplateId);
            if (hasProduction) {
                ModelState.Clear ();
                ModelState.AddModelError ("", "امکان حذف قالب وجود ندارد");
                Alert = ModelState.ModelStateAsError ();
                return RedirectToPage ("./Index");
            }
            return await base.HandlerRemove (pinfoid);
        }

        // edit by eli
        public async Task<PartialViewResult> OnGetEditAsync (long eid) {
            EditKey = eid.ToString ();
            var item = await FindAsNotTrackedAsync (eid);
            var selectedList = await _FetchAsSelectAsync ();
            return Partial ("_Create", new InputModel {
                Templates = selectedList,
                    TblTemplateId = item.TblTemplateId,
                    TemplateCount = item.TemplateCount,
                    Order = item.Order,
                    Explanation = item.Explanation,
                    EditMode = true
            });
        }

        private async Task<List<SelectListItem>> _FetchAsSelectAsync () {
            var selectedItem = await _context.TblTemplate
                .Select (x => new SelectListItem {
                    Value = x.Id.ToString (), Text = x.Code,
                }).OrderBy (x => x.Value).ToListAsync ();
            selectedItem.Insert (0, new SelectListItem () {
                Value = "0", Text = "انتخاب کنید"
            });
            return selectedItem;
        }

        public class Products {
            public long ProductId { get; set; }

            public int ProductCount { get; set; }

            public string ProductTitle { get; set; }

            public string ProductWeightAsKg { get; set; }

            public double TotalProductWeightAsKg { get; set; }
        }
    }
}
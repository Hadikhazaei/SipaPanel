using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

// 
using CldLayer.Persian;
using DbLayer.Context;
using DbLayer.DbTable;
using DbLayer.Enums;
using ZyPanel.Areas.Shared;
using ZyPanel.Helper.Utils;

namespace ZyPanel.Areas.QControl.Pages.ToDo {
    public class IndexModel : FetchRootPage<TblQControl> {
        public IndexModel (AppDbContext context) : base (context) { }

        public class InputModel {
            public DateTime ProductionDate { get; set; }

            [Display (Name = "تاریخ تولید : ")]
            [Required (ErrorMessage = ConstValues.RqError)]
            public string PersianProductionDate { get; set; }

            [StringLength (50)]
            [Display (Name = "بازرس : ")]
            public string Inspecter { get; set; }

            [Display (Name = "ایستگاه بازرسی : ")]
            [Required (ErrorMessage = ConstValues.RqError)]
            public StationType StationType { get; set; }

            [Display (Name = "تعداد مرجوعی : ")]
            [Required (ErrorMessage = ConstValues.RqError)]
            [Range (0, int.MaxValue, ErrorMessage = ConstValues.RgError)]
            public int BackCount { get; set; } = 0;

            [Display (Name = "توضیحات : ")]
            public string Explanation { get; set; }
            // 
            // 
            //
            [Display (Name = "قطعه : ")]
            [Required (ErrorMessage = ConstValues.RqError)]
            [Range (1, long.MaxValue, ErrorMessage = ConstValues.RgError)]
            public long TblProductId { get; set; }
            public List<SelectListItem> Products { get; set; }

            public long TblPlanningId { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class ListModel {
            public long Id { get; set; }

            public int WasteCount { get; set; }

            public int BackCount { get; set; }

            public string PersianCreateDate { get; set; }

            public string PersianProductionDate { get; set; }

            public string Inspecter { get; set; }

            public string StationTitle { get; set; }

            public string Explanation { get; set; }

            public string Product { get; set; }
        }

        public PaginatedList<ListModel> List { get; set; }

        public async Task OnGetAsync (int p = 1) {
            List = await PaginatedList<ListModel>.CreateAsync (
                _dbSet.Include (x => x.TblProduct)
                .Include (x => x.TblProduct).OrderByDescending (x => x.Id)
                .Select (x => new ListModel {
                    Id = x.Id,
                        WasteCount = x.WasteCount,
                        BackCount = x.BackCount,
                        PersianCreateDate = x.PersianCreatedDate,
                        PersianProductionDate = x.PersianProductionDate,
                        Inspecter = x.Inspecter,
                        StationTitle = x.StationTitle,
                        Explanation = x.Explanation,
                        Product = x.TblProduct.Title,
                }), p, _pageSize
            );
        }

        public async Task<IActionResult> OnPostAsync () {
            if (ModelState.IsValid) {
                try {
                    var productionDate = Input.PersianProductionDate
                        .ToGregorianDateTimeOrDefault ();
                    var template = await _context.JoinTP.FirstOrDefaultAsync (x => x.TblProductId == Input.TblProductId);
                    var production = await _context.TblProductionInfo
                        .FirstOrDefaultAsync (x => x.CreatedDate.Date == productionDate.Date && x.TblTemplateId == template.TblTemplateId);
                    if (production == null) {
                        var product = await _context.TblProduct.FindAsync (Input.TblProductId);
                        throw new Exception ($"در تاریخ '<bdo class='ltr'>{Input.PersianProductionDate}</bdo>' قطعه '{product.Title}' ثبت نشده است.");
                    }
                    Input.ProductionDate = productionDate;
                    Input.TblPlanningId = production.TblPlanningId;
                    await AddItemExtend<InputModel> (Input);
                    Alert = ModelStateType.A200.ModelStateAsText ();
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
            model.Products = await _context.TblProduct
                .Select (x => new SelectListItem { Value = x.Id.ToString (), Text = x.Title }).ToListAsync ();
            model.Products.Insert (0, new SelectListItem () { Value = "0", Text = "انتخاب کنید" });
            return Partial ("_Create", model);
        }

        public async Task<IActionResult> OnGetIncreaseWasteAsync (long id) {
            try {
                var item = await _dbSet.FindAsync (id);
                item.WasteCount++;
                await EditItem (item);
                return new OkObjectResult (item.WasteCount);
            } catch {
                return BadRequest (ConstValues.ErrRequest);
            }
        }
        public async Task<IActionResult> OnGetDecreaseWasteAsync (long id) {
            try {
                var item = await _dbSet.FindAsync (id);
                if (item.WasteCount <= 0) {
                    throw new Exception ("درخواست کاهش مجاز نمی باشد.");
                }
                item.WasteCount--;
                await EditItem (item);
                return new OkObjectResult (item.WasteCount);
            } catch (Exception ex) {
                return BadRequest (ex.Message);
            }
        }
    }
}
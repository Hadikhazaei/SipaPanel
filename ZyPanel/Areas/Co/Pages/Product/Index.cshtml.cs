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
using ZyPanel.Helper.Vmodel;

namespace ZyPanel.Areas.Co.Pages.Product {
    public class IndexModel : FetchRootPage<TblProduct> {
        public IndexModel (AppDbContext context) : base (context) { }

        public class InputModel {
            [Display (Name = "عنوان : ")]
            [Required (ErrorMessage = ConstValues.RqError)]
            public string Title { get; set; }

            [Display (Name = "کد انبار : ")]
            [StringLength (50, ErrorMessage = ConstValues.RgError)]
            public string StoreCode { get; set; }

            [Display (Name = "کد فنی : ")]
            [StringLength (50, ErrorMessage = ConstValues.RgError)]
            public string TechnicalCode { get; set; }

            [Display (Name = "نوع ذوب : ")]
            [Required (ErrorMessage = ConstValues.RqError)]
            [Range (1, long.MaxValue, ErrorMessage = ConstValues.RgError)]
            public long TblFusionId { get; set; }

            public List<SelectListItem> Fusions { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class ListModel {
            public long Id { get; set; }

            public string Title { get; set; }

            public string StoreCode { get; set; }

            public string TechnicalCode { get; set; }

            public string Fusion { get; set; }
        }

        public PaginatedList<ListModel> List { get; set; }

        public async Task OnGetAsync ([FromQuery] FilterQs vm) {
            var entity = _dbSet.Include (x => x.TblFusion).AsNoTracking ();
            if (!string.IsNullOrEmpty (vm.Filter)) {
                entity = _dbSet.Where (x => x.Title.Contains (vm.Filter));
            }
            List = await PaginatedList<ListModel>.CreateAsync (
                entity.OrderByDescending (x => x.Id)
                .Select (x => new ListModel {
                    Id = x.Id,
                        Title = x.Title,
                        StoreCode = x.StoreCode,
                        TechnicalCode = x.TechnicalCode,
                        Fusion = x.TblFusion.CompleteTitle
                }), vm.P, _pageSize
            );
        }

        public async Task<IActionResult> OnPostAsync () {
            if (ModelState.IsValid) {
                try {
                    if (EditKey != null) {
                        await EditItemExtend (x => x.Title,
                            x => x.StoreCode, x => x.TechnicalCode, x => x.TblFusionId);
                        return RedirectToPage ("./Index");
                    }
                    await AddItemExtend<InputModel> (Input);
                } catch (Exception ex) {
                    ModelState.AddModelError ("", ex.Message);
                    Alert = ModelState.ModelStateAsError ();
                }
            }
            return RedirectToPage ("./Index");
        }

        // handler
        public async Task<PartialViewResult> OnGetCreate () {
            var selectedList = await _FetchAsSelectAsync ();
            return Partial ("_Create", new InputModel { Fusions = selectedList });
        }

        public async Task<PartialViewResult> OnGetEditAsync (long id) {
            EditKey = id.ToString ();
            var result = await _dbSet.FirstOrDefaultAsync (x => x.Id == id);
            var selectedList = await _FetchAsSelectAsync ();
            return Partial ("_Create", new InputModel {
                Title = result.Title,
                    StoreCode = result.StoreCode,
                    TechnicalCode = result.TechnicalCode,
                    Fusions = selectedList,
                    TblFusionId = result.TblFusionId
            });
        }

        // 
        private async Task<List<SelectListItem>> _FetchAsSelectAsync () {
            var selectedItem = await _context.TblFusion
                .Select (x => new SelectListItem {
                    Value = x.Id.ToString (), Text = x.CompleteTitle,
                }).OrderBy (x => x.Value).ToListAsync ();
            selectedItem.Insert (0, new SelectListItem () {
                Value = "0", Text = "انتخاب کنید"
            });
            return selectedItem;
        }
    }
}
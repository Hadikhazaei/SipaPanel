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

namespace ZyPanel.Areas.QControl.Pages.ToDoInfo {
    public class IndexModel : FetchRootPage<TblQControlInfo> {
        public IndexModel (AppDbContext context) : base (context) { }

        [BindProperty (SupportsGet = true)]
        public long Id { get; set; }

        public class InputModel {
            [StringLength (50)]
            [Display (Name = "کد ردیابی : ")]
            public string TrackCode { get; set; }

            [StringLength (50)]
            [Display (Name = "نوع محفظه : ")]
            public string ShieldType { get; set; }

            [Display (Name = "توضیحات : ")]
            public string Explanation { get; set; }
            // 
            // 
            // 

            [Display (Name = "نامنطبق")]
            public bool IsWaste { get; set; } = false;

            [Display (Name = "ایستگاه بازرسی : ")]
            [Required (ErrorMessage = ConstValues.RqError)]
            public StationType StationType { get; set; }
            // 
            // 
            // 
            [Display (Name = "محل عیب : ")]
            [Required (ErrorMessage = ConstValues.RqError)]
            public DefectPlaceType DefectPlaceType { get; set; }

            [Display (Name = "عیب : ")]
            [Required (ErrorMessage = ConstValues.RqError)]
            [Range (1, long.MaxValue, ErrorMessage = ConstValues.RgError)]
            public long TblDefectId { get; set; }
            public List<SelectListItem> Defects { get; set; }

            // 
            public long TblQControlId { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class ListModel {
            public long Id { get; set; }

            public string Defect { get; set; }

            public string PersianCreateDate { get; set; }

            public string TrackCode { get; set; }

            public string ShieldType { get; set; }

            public string DefectPlaceTitle { get; set; }

            public bool IsWaste { get; set; }

            public string Explanation { get; set; }

        }

        public PaginatedList<ListModel> List { get; set; }

        public async Task OnGetAsync (int p = 1) {
            List = await PaginatedList<ListModel>.CreateAsync (
                _dbSet.Include (x => x.TblDefect)
                .Where (x => x.TblQControlId == Id)
                .OrderByDescending (x => x.Id)
                .Select (x => new ListModel {
                    Id = x.Id,
                        Defect = x.TblDefect.Title,
                        PersianCreateDate = x.PersianCreatedDate,
                        TrackCode = x.TrackCode,
                        ShieldType = x.ShieldType,
                        DefectPlaceTitle = x.DefectPlaceTitle,
                        IsWaste = x.IsWaste,
                        Explanation = x.Explanation
                }), p, _pageSize
            );
        }

        public async Task<IActionResult> OnPostAsync () {
            if (ModelState.IsValid) {
                try {
                    Input.TblQControlId = Id;
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
            model.Defects = await _context.TblDefect.Select (x => new {
                Value = x.Id.ToString (), Text = $"{x.DefectTitle} - {x.Title}",
            }).Select (x => new SelectListItem { Value = x.Value, Text = x.Text }).ToListAsync ();
            model.Defects.Insert (0, new SelectListItem () {
                Value = "0", Text = "انتخاب کنید"
            });
            return Partial ("_Create", model);
        }

        // public async Task<IActionResult> OnGetProductAsSelectAsync (string productionDate) {
        //     try {
        //         var result = await _GetProductAsSelectAsync (productionDate);
        //         return new OkObjectResult (result);
        //     } catch {
        //         return BadRequest (ConstValues.ErrRequest);
        //     }
        // }

        // Private
        // private async Task<List<SelectListItem>> _GetProductAsSelectAsync (string productionDate) {
        //     var _productionDate = productionDate.ToGregorianDateTimeOrDefault ();
        //     var templatesId = await _context.TblProductionInfo
        //         .Where (x => x.CreatedDate.Date == _productionDate.Date && x.HallId == Id)
        //         .Select (x => x.TblTemplateId).Distinct ().ToListAsync ();

        //     var productesId = await _context.JoinTP.Where (x => templatesId.Any (y => y == x.TblTemplateId))
        //         .Select (x => x.TblProductId).Distinct ().ToListAsync ();
        //     // 
        //     var result = await _context.TblProduct.Where (x => productesId.Any (y => y == x.Id)).Select (x => new {
        //         Value = x.Id.ToString (), Text = x.Title,
        //     }).Select (x => new SelectListItem { Value = x.Value, Text = x.Text }).ToListAsync ();
        //     result.Insert (0, new SelectListItem () { Value = "0", Text = "انتخاب کنید" });
        //     return result;
        // }

        // private async Task<List<SelectListItem>> _FetchDefectAsSelectAsync () {
        //     var hall = await _context.TblHall.FindAsync (Id);
        //     var defect = _context.TblDefect.AsQueryable ();
        //     switch (hall.HallType) {
        //         case HallType.CastIron:
        //             defect = defect.Where (x => x.DefectLineType == DefectLineType.Blok ||
        //                 x.DefectLineType == DefectLineType.NonBlok);
        //             break;
        //         case HallType.Aluminium:
        //             defect = defect.Where (x => x.DefectLineType == DefectLineType.Aluminum);
        //             break;
        //         case HallType.CNC:
        //             defect = defect.Where (x => x.DefectLineType == DefectLineType.CNC);
        //             break;
        //         default:
        //             break;
        //     }
        //     var result = await defect.Select (x => new {
        //         Value = x.Id.ToString (), Text = $"{x.DefectTitle} - {x.Title}",
        //     }).Select (x => new SelectListItem { Value = x.Value, Text = x.Text }).ToListAsync ();
        //     result.Insert (0, new SelectListItem () {
        //         Value = "0", Text = "انتخاب کنید"
        //     });
        //     return result;
        // }

        // public async Task<PartialViewResult> OnGetEditAsync (long qid) {
        //     EditKey = qid.ToString ();
        //     var item = await FindAsNotTrackedAsync (qid);
        //     var defects = await _FetchDefectAsSelectAsync ();
        //     var products = await _GetProductAsSelectAsync (item.PersianProductionDate);
        //     return Partial ("_Create", new InputModel {
        //         PersianProductionDate = item.PersianProductionDate,
        //             StationType = item.StationType,
        //             DefectPlaceType = item.DefectPlaceType,
        //             Healthy = item.Healthy,
        //             Waste = item.Waste,
        //             IsWaste = item.IsWaste,
        //             BackCount = item.BackCount,
        //             TrackCode = item.TrackCode,
        //             ShieldType = item.ShieldType,
        //             Inspecter = item.Inspecter,
        //             Explanation = item.Explanation,
        //             Products = products,
        //             TblProductId = item.TblProductId,
        //             Defects = defects,
        //             TblDefectId = item.TblDefectId
        //     });
        // }
    }
}
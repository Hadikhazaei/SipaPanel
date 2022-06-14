using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// 
using CldLayer.Persian;
using DbLayer.Context;
using DbLayer.DbTable;
using ZyPanel.Areas.Shared;
using ZyPanel.Helper.Utils;

namespace ZyPanel.Areas.Co.Pages.Template {
    public class WeightModel : FetchRootPage<JoinTP> {
        public WeightModel (AppDbContext context) : base (context) { }

        [BindProperty (SupportsGet = true)]
        public long Id { get; set; }

        public string TemplateInfo { get; set; }

        public class InputModel {
            [Display (Name = "تاریخ وزن کشی : ")]
            [Required (ErrorMessage = ConstValues.RqError)]
            public string WeightDate { get; set; }

            [Display (Name = "وزن خوشه (g) : ")]
            [Range (0, double.MaxValue, ErrorMessage = ConstValues.RgError)]
            public int TemplateWeight { get; set; }

            [Display (Name = "سیکل تولید (s) : ")]
            [Range (0, short.MaxValue, ErrorMessage = ConstValues.RgError)]
            public short ProductionCycle { get; set; } = 0;

            [Display (Name = "زمان بارریزی دستی (s) : ")]
            [Range (0, short.MaxValue, ErrorMessage = ConstValues.RgError)]
            public short ManualFusionTime { get; set; } = 0;

            [Display (Name = "زمان بارریزی پورینگ (s) : ")]
            [Range (0, short.MaxValue, ErrorMessage = ConstValues.RgError)]
            public short PoringFusionTime { get; set; } = 0;

            public List<Products> Products { get; set; }

            // extra
            public bool IsReady { get; set; }

            public string Message { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class ListModel {
            public long Id { get; set; }

            public short Time { get; set; }

            public string WeightCode { get; set; }

            public string WeightDate { get; set; }

            public string TemplateWeight { get; set; }

            // cycle
            public short ProductionCycle { get; set; }

            public short ManualFusionTime { get; set; }

            public short PoringFusionTime { get; set; }

            // product
            public string Product { get; set; }

            public long ProductId { get; set; }

            public int ProductCount { get; set; }

            public string ProductWeight { get; set; }

            // extra
            public bool IsReady { get; set; }
        }

        public List<ListModel> List { get; set; }

        public async Task OnGetAsync () {
            var template = await _context.TblTemplate.FindAsync (Id);;
            TemplateInfo = $"کد قالب : {template.Code}";
            List = await _dbSet
                .Include (x => x.TblProduct)
                .Where (x => x.TblTemplateId == Id)
                .AsNoTracking ().Select (x => new ListModel {
                    Id = x.Id,
                        WeightCode = x.WeightCode,
                        WeightDate = x.PersianWeightDate,
                        TemplateWeight = x.TemplateDisplayAsKg,
                        // 
                        ProductionCycle = x.ProductionCycle,
                        ManualFusionTime = x.ManualFusionTime,
                        PoringFusionTime = x.PoringFusionTime,
                        // 
                        Product = x.TblProduct.Title,
                        ProductId = x.TblProductId,
                        ProductCount = x.ProductCount,
                        ProductWeight = x.ProductDisplayAsKg,
                        Time = x.Time,
                        // 
                        IsReady = x.IsReady,
                }).OrderBy (x => x.IsReady ? 0 : 1).ToListAsync ();
        }

        public async Task<IActionResult> OnPostAsync () {
            if (ModelState.IsValid) {
                try {
                    var lastItem = await _dbSet
                        .OrderByDescending (x => x.Id)
                        .FirstOrDefaultAsync (x => x.TblTemplateId == Id);
                    if (lastItem == null) {
                        throw new Exception (ConstValues.ErrRequest);
                    }
                    var lastTime = lastItem.Time;
                    var template = await _context.TblTemplate.FindAsync (Id);
                    var weightDate = Input.WeightDate?.ToGregorianDateTimeOrDefault ();
                    var lastItems = await _dbSet.Where (x => x.TblTemplateId == Id && x.Time == lastTime).ToListAsync ();
                    var isFirst = !lastItem.WeightDate.HasValue;
                    var newTime = isFirst ? 1 : ++lastTime;
                    var weightCode = $"{template.Code}-{newTime}-{Input.WeightDate}";
                    // 
                    // if the fist time
                    // 
                    if (isFirst) {
                        foreach (var item in lastItems) {
                            item.WeightCode = weightCode;
                            item.WeightDate = weightDate;
                            item.TemplateWeight = Input.TemplateWeight;
                            // 
                            item.ProductionCycle = Input.ProductionCycle;
                            item.ManualFusionTime = Input.ManualFusionTime;
                            item.PoringFusionTime = Input.PoringFusionTime;
                            // 
                            item.ProductWeight = Input.Products.FirstOrDefault (x => x.ProductId == item.TblProductId).ProductEntry;
                            // 
                            item.IsReady = true;
                        }
                        _dbSet.UpdateRange (lastItems);
                        await _context.SaveChangesAsync ();
                    }
                    // 
                    // if is not the first time
                    // 
                    else {
                        foreach (var item in lastItems) {
                            item.IsReady = false;
                        }
                        _dbSet.UpdateRange (lastItems);
                        foreach (var item in Input.Products) {
                            await _dbSet.AddAsync (new JoinTP {
                                TblTemplateId = Id,
                                    WeightCode = weightCode,
                                    WeightDate = weightDate,
                                    TemplateWeight = Input.TemplateWeight,
                                    // 
                                    ProductionCycle = Input.ProductionCycle,
                                    ManualFusionTime = Input.ManualFusionTime,
                                    PoringFusionTime = Input.PoringFusionTime,
                                    // 
                                    TblProductId = item.ProductId,
                                    ProductWeight = item.ProductEntry,
                                    ProductCount = lastItems.FirstOrDefault (x => x.TblProductId == item.ProductId).ProductCount,
                                    // 
                                    IsReady = true,
                                    Time = (short) newTime,
                            });
                        }
                        await _context.SaveChangesAsync ();
                    }
                    Alert = ModelStateType.A200.ModelStateAsText ();
                } catch (Exception ex) {
                    ModelState.AddModelError ("", ex.Message);
                    Alert = ModelState.ModelStateAsError ();
                }
            }
            return RedirectToPage ("./Weight");
        }

        // handler
        public async Task<PartialViewResult> OnGetCreate () {
            var model = new InputModel ();
            var template = await _context.TblTemplate.SingleAsync (x => x.Id == Id);
            var lastItem = await _dbSet.OrderByDescending (x => x.Id)
                .FirstOrDefaultAsync (x => x.TblTemplateId == Id);
            if (lastItem == null) {
                model.Message = $"برای قالب '{template.Code}' هنوز قطعه ای در نظر گرفته نشده است.";
                return Partial ("_WeightCreate", model);
            }
            var totalProducts = await _dbSet
                .Where (x => x.TblTemplateId == Id && x.Time == lastItem.Time).SumAsync (x => x.ProductCount);
            if (template.KaviteCount == totalProducts) {
                model.Products = await _dbSet.Where (x => x.TblTemplateId == Id && x.Time == lastItem.Time)
                    .Include (x => x.TblProduct).Select (x => new Products {
                        ProductId = x.TblProductId,
                            ProductTitle = $"وزن هر {x.TblProduct.Title} (g) - تعداد : {x.ProductCount}"
                    }).ToListAsync ();
                model.IsReady = true;
            } else {
                model.Message = $"قالب '{template.Code}' دارای {template.KaviteCount} کویته می باشد و هنوز {template.KaviteCount - totalProducts} کویته آن خالی می باشد.";
            }
            return Partial ("_WeightCreate", model);
        }

        public async Task<IActionResult> OnPostRemove (short time) {
            try {
                if (await _dbSet.AnyAsync (x => x.TblTemplateId == Id && x.Time == time && x.IsReady)) {
                    throw new Exception ("امکان حذف وزن فعال مقدور نمی باشد.");
                }
                var weight = await _dbSet.FirstOrDefaultAsync (x => x.TblTemplateId == Id && x.Time == time);
                var planningInfo = await _context.TblPlanningInfo
                    .FirstOrDefaultAsync (x => x.TblTemplateId == Id && x.WeightCode == weight.WeightCode);
                if (planningInfo != null) {
                    throw new Exception ("به دلیل داشتن وابستگی امکان حذف مقدور نمی باشد.");
                }
                var weightingToRemove = await _dbSet.Where (x => x.TblTemplateId == Id && x.Time == time).ToListAsync ();
                _context.JoinTP.RemoveRange (weightingToRemove);
                await _context.SaveChangesAsync ();
                Alert = ModelStateType.A200.ModelStateAsText ();
            } catch (Exception ex) {
                ModelState.Remove ("WeightDate");
                ModelState.AddModelError ("", ex.Message);
                Alert = ModelState.ModelStateAsError ();
            }
            return RedirectToPage ("./Weight");
        }

        public class Products {
            public long ProductId { get; set; }

            [Range (0, int.MaxValue, ErrorMessage = ConstValues.RgError)]
            public int ProductEntry { get; set; }

            public string ProductTitle { get; set; }
        }
    }
}
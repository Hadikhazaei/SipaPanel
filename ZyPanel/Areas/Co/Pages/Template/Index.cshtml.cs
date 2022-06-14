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

namespace ZyPanel.Areas.Co.Pages.Template {
    public class IndexModel : FetchRootPage<TblTemplate> {
        public IndexModel (AppDbContext context) : base (context) { }

        public class InputModel {
            [Display (Name = "کد قالب : ")]
            [Required (ErrorMessage = ConstValues.RqError)]
            public string Code { get; set; }

            [Display (Name = "تعداد کویته : ")]
            [Required (ErrorMessage = ConstValues.RqError)]
            [Range (1, byte.MaxValue, ErrorMessage = ConstValues.RgError)]
            public byte KaviteCount { get; set; } = 1;

            public bool CanEditLine { get; set; } = true;

            public bool CanEditKavite { get; set; } = true;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputProductModel {
            [Display (Name = "قطعه : ")]
            [Required (ErrorMessage = ConstValues.RqError)]
            [Range (1, long.MaxValue, ErrorMessage = ConstValues.RqError)]
            public long ProductId { get; set; }

            public List<SelectListItem> Products { get; set; }

            [Display (Name = "تعداد : ")]
            [Range (1, short.MaxValue, ErrorMessage = ConstValues.RqError)]
            public short Count { get; set; } = 1;

            public string Message { get; set; }

            public bool IsAvailabel { get; set; }

            // 
            public long TemplateId { get; set; }
        }

        [BindProperty]
        public InputProductModel InputProduct { get; set; }

        public class ListModel {
            public long Id { get; set; }

            public string Hall { get; set; }

            public string Code { get; set; }

            public bool IsActive { get; set; }

            public bool IsReady { get; set; }

            public string WeightingDate { get; set; }

            public string TemplateWeight { get; set; }

            public byte KaviteCount { get; set; }

            public List<Products> Products { get; set; }

            public double TotalProductWeight { get; set; }
        }

        public class Products {
            public long Id { get; set; }
            public int ProductCount { get; set; }
            public string ProductTitle { get; set; }
            public string ProductWeight { get; set; }
        }

        public PaginatedList<ListModel> List { get; set; }

        public async Task OnGetAsync ([FromQuery] FilterQs vm) {
            var entity = _dbSet
                .Include (x => x.TblHallTemplate)
                .Include (x => x.TblHallTemplate)
                .Include (x => x.JoinTP.Where (x => x.IsReady))
                .ThenInclude (x => x.TblProduct).AsNoTracking ();
            if (!string.IsNullOrEmpty (vm.Filter)) {
                entity = _dbSet.Where (x => x.Code.Contains (vm.Filter.Trim ()));
            }
            List = await PaginatedList<ListModel>.CreateAsync (
                entity.Select (x => new ListModel {
                    Id = x.Id,
                        Code = x.Code,
                        IsActive = x.IsActive,
                        KaviteCount = x.KaviteCount,
                        TemplateWeight = x.JoinTP.FirstOrDefault (x => x.IsReady).TemplateDisplayAsKg,
                        WeightingDate = x.JoinTP.FirstOrDefault (x => x.IsReady).PersianWeightDate,
                        IsReady = x.JoinTP.Where (x => x.IsReady).Sum (x => x.ProductCount) == x.KaviteCount && x.TblHallTemplate.Any (),
                        Hall = String.Join (',', x.TblHallTemplate.Select (x => x.TblHall.FullTitle)),
                        Products = x.JoinTP.Where (x => x.IsReady).Select (p => new Products {
                            Id = p.Id,
                                ProductCount = p.ProductCount,
                                ProductTitle = p.TblProduct.Title,
                                ProductWeight = p.ProductDisplayAsKg
                        }).ToList (),
                        TotalProductWeight = ((double) x.JoinTP.Where (x => x.IsReady).Sum (x => x.ProductCount * x.ProductWeight) / 1000)
                }), vm.P, _pageSize);
        }

        public async Task<IActionResult> OnPostAsync () {
            try {
                ModelState.MarkAllFieldsAsSkipped ();
                if (!TryValidateModel (Input, nameof (Input))) {
                    throw new Exception (ConstValues.ErrRequest);
                }
                var entryCode = Input.Code.Trim ();
                var template = await _dbSet.FirstOrDefaultAsync (x => x.Code == entryCode);
                if (EditKey != null) {
                    var editKey = long.Parse (EditKey);
                    var item = await FindAsync (editKey);
                    if (item.Code != Input.Code && template != null) {
                        throw new Exception ($"کد '{template.Code}' قالب موجود می باشد.");
                    }
                    if (await TryUpdateModelAsync<TblTemplate> (item, "",
                            x => x.Code, x => x.KaviteCount
                        )) {
                        await _context.SaveChangesAsync ();
                    }
                } else {
                    if (template != null) {
                        throw new Exception ($"کد '{template.Code}' قالب موجود می باشد.");
                    }
                    await AddItemExtend<InputModel> (Input);
                    // var entity = new TblTemplate ();
                    // var entry = await _context.AddAsync (entity);
                    // entry.CurrentValues.SetValues (Input);
                    // await _context.SaveChangesAsync ();
                }
                Alert = ModelStateType.A200.ModelStateAsText ();
            } catch (Exception ex) {
                ModelState.AddModelError ("", ex.Message);
                Alert = ModelState.ModelStateAsError ();
            }
            return RedirectToPage ("./Index");
        }

        // handler
        public PartialViewResult OnGetCreate () => base.HandlerCreatePartial<InputModel> ();

        public async Task<PartialViewResult> OnGetEditAsync (long templateId) {
            EditKey = templateId.ToString ();
            var result = await _dbSet.FirstOrDefaultAsync (x => x.Id == templateId);
            var canEditKavite = await _context.JoinTP.AnyAsync (x => x.TblTemplateId == templateId);
            var canEditLine = await _context.TblPlanningInfo.AnyAsync (x => x.TblTemplateId == templateId);
            return Partial ("_Create", new InputModel {
                Code = result.Code,
                    KaviteCount = result.KaviteCount,
                    CanEditKavite = !canEditKavite,
                    CanEditLine = !canEditLine,
            });
        }

        public async Task<IActionResult> OnGetActiveAsync (long id) {
            try {
                var item = await _dbSet.FindAsync (id);
                item.IsActive = !item.IsActive;
                await EditItem (item);
                return new OkObjectResult (ConstValues.OkRequest);
            } catch {
                return BadRequest (ConstValues.ErrOnFetchingData);
            }
        }

        // product
        public async Task<IActionResult> OnPostProductAsync (InputProductModel model) {
            ModelState.MarkAllFieldsAsSkipped ();
            if (TryValidateModel (model, nameof (model))) {
                try {
                    var template = await _dbSet.SingleAsync (x => x.Id == model.TemplateId);
                    var isReady = await _context.JoinTP.AnyAsync (x => x.TblTemplateId == model.TemplateId && x.IsReady);
                    if (isReady) {
                        throw new Exception ($"شما مجاز به حداکثر {template.KaviteCount} قطعه برای قالب '{template.Code}' می باشید.");
                    }
                    var counts = await _context.JoinTP.Where (x => x.TblTemplateId == model.TemplateId).SumAsync (x => x.ProductCount);
                    counts += model.Count;
                    if (counts > template.KaviteCount) {
                        throw new Exception ($"شما مجاز به حداکثر {template.KaviteCount} قطعه برای قالب '{template.Code}' می باشید.");
                    }
                    _context.JoinTP.Add (new JoinTP {
                        ProductCount = model.Count,
                            TblProductId = model.ProductId,
                            TblTemplateId = model.TemplateId
                    });
                    await _context.SaveChangesAsync ();
                    Alert = ModelStateType.A200.ModelStateAsText ();
                } catch (Exception ex) {
                    ModelState.AddModelError ("", ex.Message);
                    Alert = ModelState.ModelStateAsError ();
                }
                return RedirectToPage ("./Index");
            }
            Alert = ModelStateType.A400.ModelStateAsText ();
            return RedirectToPage ("./Index");
        }

        public async Task<PartialViewResult> OnGetCreateProductAsync (long templateId) {
            var model = new InputProductModel {
                IsAvailabel = false
            };
            var template = await _dbSet.SingleAsync (x => x.Id == templateId);
            var hasPlanningInfo = await _context.TblPlanningInfo.AnyAsync (x => x.TblTemplateId == templateId);
            if (hasPlanningInfo) {
                model.Message = $"امکان افزودن قطعه در قالب با کد '{template.Code}' امکان پذیر نمی باشد!";
                return Partial ("_ProductCreate", model);
            }
            var productCount = await _context.JoinTP.Where (x => x.TblTemplateId == templateId)
                .Select (x => new { x.TblProductId, x.ProductCount }).Distinct ().SumAsync (x => x.ProductCount);
            if (productCount == template.KaviteCount) {
                model.Message = $"تعداد کویته های قالب '{template.Code}' پر شده است!";
                return Partial ("_ProductCreate", model);
            }
            var selectedList = await _context.TblProduct
                .Where (x => !x.JoinTP.Any (x => x.TblTemplateId == templateId)).AsNoTracking ()
                .Select (x => new SelectListItem { Value = x.Id.ToString (), Text = $"{x.Title} , کد : {x.Id}", }).ToListAsync ();
            selectedList.Insert (0, new SelectListItem () { Value = "0", Text = "انتخاب کنید" });
            model.IsAvailabel = true;
            model.Products = selectedList;
            model.TemplateId = templateId;
            return Partial ("_ProductCreate", model);
        }
    }
}
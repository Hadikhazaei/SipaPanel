using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// 
using DbLayer.Context;
using DbLayer.DbTable;
using DbLayer.Enums;
using ZyPanel.Areas.Shared;
using ZyPanel.Helper.Utils;

namespace ZyPanel.Areas.Co.Pages.Fusion {
    public class IndexModel : FetchRootPage<TblFusion> {
        public IndexModel (AppDbContext context) : base (context) { }

        public class InputModel {
            [Display (Name = "عنوان : ")]
            [Required (ErrorMessage = ConstValues.RqError)]
            public string Title { get; set; }

            [Display (Name = "نوع ذوب : ")]
            [Required (ErrorMessage = ConstValues.RqError)]
            public FusionType FusionType { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class ListModel {
            public long Id { get; set; }

            public string Fusion { get; set; }

            public string Title { get; set; }
        }

        public List<ListModel> List { get; set; }

        public async Task OnGetAsync () {
            List = await _dbSet
                .Select (x => new ListModel {
                    Id = x.Id,
                        Title = x.Title,
                        Fusion = x.TypeTitle,
                }).ToListAsync ();
        }

        public async Task<IActionResult> OnPostAsync () {
            if (ModelState.IsValid) {
                try {
                    if (EditKey != null) {
                        await EditItemExtend (x => x.Title, x => x.FusionType);
                        return RedirectToPage ("./Index");
                    } else {
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
        public PartialViewResult OnGetCreate () => base.HandlerCreatePartial<InputModel> ();

        public async Task<PartialViewResult> OnGetEditAsync (long id) {
            var item = await FindAsNotTrackedAsync (id);
            return base.HandlerEditPartial<InputModel> (item);
        }
    }
}
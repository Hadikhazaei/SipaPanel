using System;
using System.Collections.Generic;
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

namespace ZyPanel.Areas.Production.Pages.ToMake {
    public class InfoModel : ModifyRootPage<TblProductionInfo> {
        public InfoModel (AppDbContext context) : base (context, null, "./Info") { }

        [BindProperty (SupportsGet = true)]
        public long Id { get; set; }

        [BindProperty (SupportsGet = true)]
        public string WeightCode { get; set; }

        [BindProperty (SupportsGet = true)]
        public string TemplateCode { get; set; }

        public class ListModel {
            public long Id { get; set; }

            public string Schedule { get; set; }

            public string UserId { get; set; }

            public string ChargeType { get; set; }

            public int PutCount { get; set; }

            public int TakeCount { get; set; }

            public string DateCreate { get; set; }

            public DateTime CreatedDate { get; set; }
        }

        public List<ListModel> List { get; set; }

        public class ProductModel {
            public string Key { get; set; }

            public string Value { get; set; }
        }

        public List<ProductModel> ProductList { get; set; }

        public async Task<IActionResult> OnGetAsync (int p = 1) {
            if (!await _context.TblPlanning.AnyAsync (x => x.Id == Id)) {
                return NotFound ();
            }
            // planning
            var joinTp = await _context.JoinTP
                .Where (x => x.WeightCode == WeightCode)
                .Include (x => x.TblProduct).Select (x => new {
                    productTitle = x.TblProduct.Title,
                        productWeight = x.ProductDisplayAsKg,
                        templateId = x.TblTemplateId,
                        clusterAsKg = x.TemplateDisplayAsKg
                }).ToListAsync ();
            ProductList = joinTp
                .Select (x => new ProductModel {
                    Key = x.productTitle, Value = x.productWeight
                }).ToList ();
            ProductList.Insert (0, new ProductModel {
                Key = $"کد قالب :{TemplateCode}",
                    Value = $"وزن خوشه : {joinTp.First().clusterAsKg}"
            });
            List = await PaginatedList<ListModel>.CreateAsync (
                _dbSet.Where (x => x.TblPlanningId == Id &&
                    x.TblTemplateId == joinTp.First ().templateId)
                .Include (x => x.AppUser)
                .Select (x => new ListModel {
                    Id = x.Id,
                        Schedule = x.ScheduleType == ScheduleType.Single ? "شیفت اول" : "شیفت دوم",
                        UserId = x.AppUser.UserName,
                        ChargeType = x.ChargeTitle,
                        PutCount = x.PutCount,
                        TakeCount = x.TakeCount,
                        DateCreate = x.PersianCreatedDate,
                        CreatedDate = x.CreatedDate
                }).OrderByDescending (x => x.Id), p, _pageSize
            );
            return Page ();
        }

        public async Task<IActionResult> OnPostRemove (long id) => await base.HandlerRemove (id);
    }
}
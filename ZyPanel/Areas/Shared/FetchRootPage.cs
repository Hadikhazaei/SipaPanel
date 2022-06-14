using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

// 
using DbLayer.Context;
using DbLayer.DbTable.Base;
using DbLayer.Interface;
using HpLayer.Service;
using ZyPanel.Helper.Utils;
using ZyPanel.Helper.Utils.Interface;

namespace ZyPanel.Areas.Shared {
    public class FetchRootPage<T> : ModifyRootPage<T> where T : KeyEntity, new () {
        public FetchRootPage () : base () { }

        public FetchRootPage (AppDbContext context, string cName = null, string rUrl = null):
            base (context, cName, rUrl) { }

        public FetchRootPage (AppDbContext context, IUploadServices uploadServices, string cName = null, string rUrl = null):
            base (context, uploadServices, cName, rUrl) { }

        #region :: Fetch ::
        protected async Task<T> FindAsync (long id) =>
            await _dbSet.FindAsync (id);

        protected async Task<T> FindAsNotTrackedAsync (long id) =>
            await _dbSet.AsNoTracking ().FirstOrDefaultAsync (x => x.Id == id);

        protected async Task<T> FirstAsync () =>
            await _dbSet.FirstOrDefaultAsync ();

        protected async Task<T> FirstAsync (Expression<Func<T, bool>> predicate) =>
            await _dbSet.FirstOrDefaultAsync (predicate);
        #endregion
    }

}
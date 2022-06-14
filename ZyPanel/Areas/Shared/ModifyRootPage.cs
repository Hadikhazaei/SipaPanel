using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

// 
using CldLayer.Persian;
using DbLayer.Context;
using DbLayer.Interface;
using HpLayer.Service;
using ZyPanel.Helper.Utils;
using ZyPanel.Helper.Utils.Interface;

namespace ZyPanel.Areas.Shared {
    public class ModifyRootPage<T> : PageModel where T : class, new () {
        // public
        public DbSet<T> _dbSet;
        public readonly int _pageSize = 10;

        public readonly AppDbContext _context;

        public readonly IUploadServices _uploadServices;

        // private
        private string _cName = "_Create", _rUrl = "./Index";

        private string fileUrl = null, thumbnailsUrl = null;

        public ModifyRootPage () { }

        public ModifyRootPage (AppDbContext context, string cName = null, string rUrl = null) {
            _context = context;
            _dbSet = _context.Set<T> ();
            _rUrl = rUrl ?? _rUrl;
            _cName = cName ?? _cName;
        }

        public ModifyRootPage (AppDbContext context, IUploadServices uploadServices, string cName, string rUrl) {
            _context = context;
            _dbSet = _context.Set<T> ();
            _uploadServices = uploadServices;
            _rUrl = rUrl ?? _rUrl;
            _cName = cName ?? _cName;
        }

        [TempData]
        public string Alert { get; set; }

        [TempData]
        public string EditKey { get; set; }

        #region :: Modify ::
        // Add
        protected async Task AddItem (T entity) {
            await _dbSet.AddAsync (entity);
            await _context.SaveChangesAsync ();
        }

        protected async Task EditItemxxx (T entity) {
            // if (typeof (ITrackEntity).IsAssignableFrom (typeof (T))) {
            //     ((ITrackEntity) entity).UpdatedDate = DateTime.UtcNow;
            // }
            _dbSet.Update (entity);
            await _context.SaveChangesAsync ();
        }

        protected async Task AddItemExtend<I> (I input) {
            if (typeof (IFileVm).IsAssignableFrom (typeof (I))) {
                var item = (IFileVm) input;
                if (item.FormFile != null) {
                    var result = await _uploadServices.SavePostedFileAsync (item.FormFile);
                    item.FileUrl = fileUrl = result?.Item1;
                    item.ThumbnailsUrl = thumbnailsUrl = result?.Item2;
                }
            }
            //TODO: add persian date
            var entity = new T ();
            var entry = await _context.AddAsync (entity);
            entry.CurrentValues.SetValues (input);
            await _context.SaveChangesAsync ();
            Alert = ModelStateType.A200.ModelStateAsText ();
        }

        protected async Task AddItemExtendNonSaveChange<I> (I input) {
            if (typeof (IFileVm)
                .IsAssignableFrom (typeof (I))) {
                var item = (IFileVm) input;
                if (item.FormFile != null) {
                    var result = await _uploadServices.SavePostedFileAsync (item.FormFile);
                    item.FileUrl = fileUrl = result?.Item1;
                    item.ThumbnailsUrl = thumbnailsUrl = result?.Item2;
                }
            }
            var entity = new T ();
            var entry = await _context.AddAsync (entity);
            entry.CurrentValues.SetValues (input);
        }

        // Edit
        protected async Task EditItem (T entity) {
            // if (typeof (ITrackEntity).IsAssignableFrom (typeof (T))) {
            //     ((ITrackEntity) entity).UpdatedDate = DateTime.UtcNow;
            // }
            _dbSet.Update (entity);
            await _context.SaveChangesAsync ();
        }

        protected async Task EditItemExtend (params Expression<Func<T, object>>[] fields) {
            var editKey = long.Parse (EditKey);
            var item = await _dbSet.FindAsync (editKey);
            if (await TryUpdateModelAsync<T> (item, "", fields)) {
                await _context.SaveChangesAsync ();
            }
            Alert = ModelStateType.A200.ModelStateAsText ();
        }

        // Remove
        protected async Task RemoveItem (object id) {
            var entity = await _dbSet.FindAsync (id);
            await RemoveItem (entity);
        }

        protected async Task RemoveItem (IEnumerable<T> entities) {
            foreach (var item in entities) {
                await RemoveItem (item);
            }
        }

        protected async Task RemoveItem (T entity) {
            if (typeof (IFileEntity).IsAssignableFrom (typeof (T))) {
                var item = ((IFileEntity) entity);
                fileUrl = item.FileUrl;
                var thumbnailsUrl = item.ThumbnailsUrl;
                _dbSet.Remove (entity);
                await _context.SaveChangesAsync ();
                if (!string.IsNullOrEmpty (fileUrl)) {
                    _uploadServices.PhysicalDeleteFile (fileUrl);
                }
                if (!string.IsNullOrEmpty (thumbnailsUrl)) {
                    _uploadServices.PhysicalDeleteFile (thumbnailsUrl);
                }
                return;
            }
            _dbSet.Remove (entity);
            await _context.SaveChangesAsync ();
        }
        #endregion

        #region :: Handler ::
        protected async Task<IActionResult> HandlerRemove (object pk) {
            try {
                await RemoveItem (pk);
                Alert = ModelStateType.A200.ModelStateAsText ();
            } catch (Exception ex) {
                ModelState.AddModelError ("", ex.Message);
                Alert = ModelState.ModelStateAsError ();
            }
            return RedirectToPage (_rUrl);
        }

        protected PartialViewResult HandlerCreatePartial<I> () where I : new () {
            return Partial (_cName, new I ());
        }

        protected PartialViewResult HandlerEditPartial<I> (T item) {
            var config = new MapperConfiguration (cfg => { cfg.CreateMap<T, I> (); });
            var resultMapping = config.CreateMapper ().Map<I> (item);
            return Partial (_cName, resultMapping);
        }
        #endregion
    }
}

#region :: Comment ::
//   protected async Task<IActionResult> AddWithCheckState<I> (I input) {
//     if (ModelState.IsValid) {
//         try {
//             if (typeof (IFileVm).IsAssignableFrom (typeof (I))) {
//                 var item = ((IFileVm) input);
//                 var result = await _uploadServices
//                     .SavePostedFileAsync (item.FormFile);
//                 item.FileUrl = fileUrl = result?.Item1;
//                 item.ThumbnailsUrl = thumbnailsUrl = result?.Item2;
//             }
//             var entity = new T ();
//             var entry = await _context.AddAsync (entity);
//             entry.CurrentValues.SetValues (input);
//             await _context.SaveChangesAsync ();
//             Alert = ModelStateType.A200.ModelStateAsText ();
//         } catch (DbUpdateException ex) {
//             if (!string.IsNullOrEmpty (fileUrl)) {
//                 _uploadServices.PhysicalDeleteFile (fileUrl);
//             }
//             if (!string.IsNullOrEmpty (thumbnailsUrl)) {
//                 _uploadServices.PhysicalDeleteFile (thumbnailsUrl);
//             }
//             ModelState.AddModelError ("", ex.Message);
//             Alert = ModelState.ModelStateAsError ();
//         }
//     } else {
//         ModelState.AddModelError ("", ConstValues.ErrRequest);
//         Alert = ModelState.ModelStateAsError ();
//     }
//     return RedirectToPage (_pgAddr.redirectUrl);
// }

// public IQueryable<T> List (Expression<Func<T, bool>> expression) {
//     return _dbSet.Where (expression);
// }

// public IQueryable<T> ByInclues (params Expression<Func<T, object>>[] includes) {
//     var result = _dbSet.AsQueryable ();
//     foreach (var includeExpression in includes)
//         result = result.Include (includeExpression);
//     return result;
// }

// public virtual T Fetch () {
//     throw new NotImplementedException ();
// }

// public async Task<Tuple<string, string>> UploadFile (IFormFile formFile, UploadSize uploadSize) =>
//     await _uploadServices.SavePostedFileAsync (formFile, uploadSize);

// public async Task<T> LatestAsync () =>
//     await _dbSet.OrderByDescending (x => x.Id).FirstOrDefaultAsync ();

// Paginate
// public async Task<List<T>> PaginatedAsync (int page = 1) {
//     var result = await _dbSet
//         .OrderBy (x => x.Id)
//         .Skip ((page - 1) * _pageSize)
//         .Take (_pageSize).ToListAsync ();
//     await SetPageCount ();
//     return result;
// }

// public async Task<List<T>> PaginatedDescAsync (int page = 1) {
//     var result = await _dbSet
//         .OrderByDescending (x => x.Id)
//         .Skip ((page - 1) * _pageSize)
//         .Take (_pageSize).ToListAsync ();
//     await SetPageCount ();
//     return result;
// }

// public async Task<List<T>> PaginatedDescAsync (IQueryable<T> queryable, int page = 1) {
//     var result = await queryable
//         .OrderByDescending (x => x.Id)
//         .Skip ((page - 1) * _pageSize)
//         .Take (_pageSize).ToListAsync ();
//     await SetPageCount ();
//     return result;
// }

//    public async Task<List<T>> PaginatedDescAsync (Expression<Func<T, bool>> where, int page = 1) {
//             var result = await _dbSet
//                 .Where (where)
//                 .OrderByDescending (x => x.Id)
//                 .Skip ((page - 1) * _pageSize)
//                 .Take (_pageSize).ToListAsync ();
//             return result;
//         }

// private async Task SetPageCount () {
//     var count = await _dbSet.CountAsync ();
//     PageCount = (int) Math.Ceiling (((double) count / _pageSize));
// }
#endregion
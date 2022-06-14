using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

// 
using DbLayer.Context;
using DbLayer.DbTable.Identity;

namespace ZyPanel.Service.IdentityService {
    public class AppUserManager : UserManager<AppUser>, IAppUserManager {
        const int pageSize = 12;
        private readonly DbSet<AppUser> _users;
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        public AppUserManager (
            AppDbContext context,
            IHttpContextAccessor contextAccessor,
            // 
            IUserStore<AppUser> appUserStore,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<AppUser> passwordHasher,
            IEnumerable<IUserValidator<AppUser>> userValidators,
            IEnumerable<IPasswordValidator<AppUser>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider serviceProvider,
            ILogger<AppUserManager> logger) : base (
            appUserStore, optionsAccessor, passwordHasher,
            userValidators, passwordValidators, keyNormalizer,
            errors, serviceProvider, logger) {

            _context = context;
            _users = _context.Set<AppUser> ();

            // 
            _contextAccessor = contextAccessor;
        }

        #region :: Extra methods ::
        public async Task<AppUser> GetCurrentUser () {
            return await FindByIdAsync (loggedUserId);
        }

        public async Task<bool> HasPhoneNumberAsync (string userId) {
            var user = await FindByIdAsync (userId);
            return user?.PhoneNumber != null;
        }

        // public async Task<PaginatedList<UserVm>> FetchPaginatedUserAsync (FilterQs vm) {
        //     var users = _users.AsNoTracking ();
        //     if (!string.IsNullOrEmpty (vm.Filter)) {
        //         users = users.Where (x => x.UserName.Contains (vm.Filter));
        //     }
        //     return await PaginatedList<UserVm>.CreateAsync (
        //         users.Select (x => new UserVm {
        //             Id = x.Id, DisplayName = x.DisplayName,
        //         }), vm.P, pageSize
        //     );
        // }
        #endregion

        #region :: Private
        private string loggedUserId => _contextAccessor.HttpContext.User.Identity.GetUserId ();
        // private string GetCurrentUserName => _contextAccessor.HttpContext.User.Identity.GetUserName ();
        #endregion
    }
}
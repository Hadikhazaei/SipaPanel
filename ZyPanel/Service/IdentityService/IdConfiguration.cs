using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

// 
using DbLayer.Context;
using DbLayer.DbTable.Identity;

namespace ZyPanel.Service.IdentityService {
    public static class IdConfiguration {
        public static IServiceCollection AddIdentityConfigure (this IServiceCollection service) {
            // DI
            // Identity
            service.AddIdentity<AppUser, IdentityRole> (options => {
                    // User
                    options.User.RequireUniqueEmail = false;
                    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-";

                    // Sign in
                    options.SignIn.RequireConfirmedEmail = false;
                    options.SignIn.RequireConfirmedPhoneNumber = false;
                    options.SignIn.RequireConfirmedAccount = false;

                    // Lock out
                    options.Lockout.AllowedForNewUsers = true;
                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes (5);

                    // Password
                    options.Password.RequiredLength = 5;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireDigit = false;
                })
                .AddUserManager<UserManager<AppUser>> ()
                .AddSignInManager<SignInManager<AppUser>> ()
                .AddUserValidator<IdUserValidator> ()
                .AddErrorDescriber<IdErrorDescriber> ()
                .AddPasswordValidator<IdPasswordValidator> ()
                .AddEntityFrameworkStores<AppDbContext> ()
                .AddDefaultTokenProviders ();
            // configure cookie
            service.ConfigureApplicationCookie (options => {
                options.Cookie.HttpOnly = true;
                options.Cookie.Name = "MaliblCookie";
                // 
                options.LoginPath = "/";
                options.LogoutPath = "/";
                options.AccessDeniedPath = "/Denied";
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromDays (30);
                // var provider = service.BuildServiceProvider ();
                // ApplicationCookieOptions (provider, optionsCookies);
            });
            return service;
        }
    }
}
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

//
using DbLayer.Context;
using DbLayer.Enums;
using HpLayer.Service;
using ZyPanel.Service.IdentityService;

namespace ZyPanel {
    public class Startup {
        public Startup (IConfiguration configure) {
            Configuration = configure;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {
            // context
            services.AddDbContext<AppDbContext> (options => {
                options.UseSqlServer (
                    Configuration["ConnectionStrings:DefaultConnection"],
                    optionsBuilder => {
                        optionsBuilder.MigrationsAssembly (typeof (Startup).Assembly.FullName);
                    }
                );
            });

            // identity
            services.AddIdentityConfigure ();

            //
            services.AddSingleton<IUploadServices, UploadServices> ();

            // 
            services.AddAuthorization (options => {
                options.AddPolicy (nameof (PolicyType.CoPolicy), policy => {
                    policy.RequireRole (
                        nameof (RoleType.CoRole),
                        nameof (RoleType.PlanningRole));
                });
                options.AddPolicy (nameof (PolicyType.ProductionPolicy), policy => {
                    policy.RequireRole (
                        nameof (RoleType.CoRole),
                        nameof (RoleType.PlanningRole),
                        nameof (RoleType.ProductionManagerRole),
                        nameof (RoleType.ProductionClerkRole));
                });
                options.AddPolicy (nameof (PolicyType.QControlPolicy), policy => {
                    policy.RequireRole (
                        nameof (RoleType.CoRole),
                        nameof (RoleType.PlanningRole),
                        nameof (RoleType.QControlRole));
                });
            });

            // razor page
            services.AddRazorPages (options => {
                // 
                options.Conventions.AuthorizeAreaFolder ("Co", "/", nameof (PolicyType.CoPolicy));
                // 
                options.Conventions.AuthorizeAreaFolder ("Planning", "/", nameof (PolicyType.CoPolicy));
                // 
                options.Conventions.AuthorizeAreaFolder ("Production", "/", nameof (PolicyType.ProductionPolicy));
                // 
                options.Conventions.AuthorizeAreaFolder ("QControl", "/", nameof (PolicyType.QControlPolicy));
            });

            // route configure
            services.Configure<RouteOptions> (options => {
                options.LowercaseUrls = true;
                options.AppendTrailingSlash = true;
                options.LowercaseQueryStrings = true;
            });

            services.AddAntiforgery (opts => opts.Cookie.Name = "anticsrf");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            } else {
                app.UseExceptionHandler ("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts ();
            }
            app.UseStatusCodePagesWithReExecute ("/Ooops/");

            app.UseHttpsRedirection ();
            app.UseStaticFiles ();
            app.UseRouting ();
            app.UseAuthentication ();
            app.UseAuthorization ();

            app.UseEndpoints (endpoints => {
                endpoints.MapRazorPages ();
            });
        }
    }
}
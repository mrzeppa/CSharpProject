using System;
using System.Threading.Tasks;
using LibraryProject.Data;
using LibraryProject.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LibraryProject
{
    public class Startup {
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {
            services.AddDbContext<ApplicationDbContext> (options =>
                options.UseSqlServer (Configuration.GetConnectionString ("ApplicationDbContext")));
            services.AddDefaultIdentity<ApplicationUser> (options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole> ()
                .AddEntityFrameworkStores<ApplicationDbContext> ();
            services.AddControllersWithViews ();
            services.AddRazorPages ();
            services.AddAuthorization (options => {
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider) {
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
                app.UseDatabaseErrorPage ();
            } else {
                app.UseExceptionHandler ("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts ();
            }
            app.UseHttpsRedirection ();
            app.UseStaticFiles ();

            app.UseRouting ();

            app.UseAuthentication ();
            app.UseAuthorization ();

            app.UseEndpoints (endpoints => {
                endpoints.MapControllerRoute (
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages ();
            });

            CreateRolesAndAdminUser (serviceProvider);
        }

        private static void CreateRolesAndAdminUser (IServiceProvider serviceProvider) {
            const string adminRoleName = "Administrator";
            string[] roleNames = { adminRoleName, "User", "Manager" };

            foreach (string roleName in roleNames)
            {
                CreateRole (serviceProvider, roleName);
            }

            AddUserToRole(serviceProvider, adminRoleName);
        }

        private static void CreateRole (IServiceProvider serviceProvider, string roleName)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            Task<bool> roleExists = roleManager.RoleExistsAsync (roleName);
            roleExists.Wait ();

            if (!roleExists.Result) {
                Task<IdentityResult> roleResult = roleManager.CreateAsync (new IdentityRole (roleName));
                roleResult.Wait ();
            }
        }

        private static void AddUserToRole (IServiceProvider serviceProvider, string adminRoleName)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var checkAppUser = userManager.FindByEmailAsync("admin@admin.pl");
            checkAppUser.Wait ();

            var appUser = checkAppUser.Result;

            if (checkAppUser.Result == null)
            {
                var newAppUser = new ApplicationUser
                {
                    Email = "admin@admin.pl",
                    UserName = "admin@admin.pl"
                };

                var taskCreateAppUser = userManager.CreateAsync(newAppUser, "zaq1@WSX");
                taskCreateAppUser.Wait();

                if (taskCreateAppUser.Result.Succeeded) {
                    appUser = newAppUser;
                }
            }
            var newUserRole = userManager.AddToRoleAsync(appUser, adminRoleName);
            newUserRole.Wait();
        }
    }
}
using System;
using System.Threading.Tasks;
using LibraryProject.Data;
using LibraryProject.Models;
using LibraryProject.Repository;
using LibraryProject.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Session;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;

namespace LibraryProject
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("ApplicationDbContext")));
            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddAuthorization(options => {
            });

            services.AddDistributedMemoryCache();
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(10);
                options.Cookie.Name = "Books";
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IBookRepository, BookRepository>();

            services.AddScoped<IBookService, BookService>();

            services.AddScoped<IAuthorRepository, AuthorRepository>();

            services.AddScoped<IAuthorService, AuthorService>();

            services.AddScoped<IBooksOfAuthorService, BooksOfAuthorService>();

            services.AddScoped<IBooksOfAuthorRepository, BooksOfAuthorRepository>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseExceptionHandler(a => a.Run(async context => {
                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                var isAdmin = context.User.IsInRole("Administrator");
                var result = isAdmin ? exceptionHandlerPathFeature.Error.ToString() : "Server side error has occured";
                await context.Response.WriteAsync(result);
            }));

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute("allBooks", "AllBooks",
                        defaults: new { controller = "Books", action = "Index" });
                routes.MapRoute("editBooks", "EditBook/{*id}",
                        defaults: new { controller = "Books", action = "Edit" });
                routes.MapRoute("booksDetails", "BookInfo/{*id}",
                        defaults: new { controller = "Books", action = "Details" });
                routes.MapRoute("deleteBook", "DeleteBook/{*id}",
                        defaults: new { controller = "Books", action = "Delete" });
                routes.MapRoute("lastSeen", "SeenRecently",
                        defaults: new { controller = "Books", action = "LastSeen" });
                routes.MapRoute("createBook", "CreateAuthorAsync",
                        defaults: new { controller = "Books", action = "Create" });

                routes.MapRoute("allAuthors", "AllAuthors",
                        defaults: new { controller = "Authors", action = "Index" });
                routes.MapRoute("editAuthor", "EditAuthor/{*id}",
                        defaults: new { controller = "Authors", action = "Edit" });
                routes.MapRoute("authorDetails", "AuthorInfo/{*id}",
                        defaults: new { controller = "Authors", action = "Details" });
                routes.MapRoute("deleteAuthor", "DeleteAuthor/{*id}",
                        defaults: new { controller = "Authors", action = "Delete" });
                routes.MapRoute("createAuthor", "CreateAuthor",
                        defaults: new { controller = "Authors", action = "Create" });

                routes.MapRoute("booksOfAuthorList", "BOA",
                        defaults: new { controller = "BooksOfAuthor", action = "Index" });
                routes.MapRoute("booksOfAuthor", "BooksOfAuthorShow",
                        defaults: new { controller = "BooksOfAuthor", action = "Show" });

                routes.MapRoute("appAuthor", "Author",
                        defaults: new { controller = "AppAuthor", action = "Index" });

                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");

            });

            CreateRolesAndAdminUser(serviceProvider);
        }

        private static void CreateRolesAndAdminUser(IServiceProvider serviceProvider)
        {
            const string adminRoleName = "Administrator";
            string[] roleNames = { adminRoleName, "User", "Manager" };

            foreach (string roleName in roleNames)
            {
                CreateRole(serviceProvider, roleName);
            }

            AddUserToRole(serviceProvider, adminRoleName);
        }

        private static void CreateRole(IServiceProvider serviceProvider, string roleName)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            Task<bool> roleExists = roleManager.RoleExistsAsync(roleName);
            roleExists.Wait();

            if (!roleExists.Result)
            {
                Task<IdentityResult> roleResult = roleManager.CreateAsync(new IdentityRole(roleName));
                roleResult.Wait();
            }
        }

        private static void AddUserToRole(IServiceProvider serviceProvider, string adminRoleName)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var checkAppUser = userManager.FindByEmailAsync("admin@admin.pl");
            checkAppUser.Wait();

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

                if (taskCreateAppUser.Result.Succeeded)
                {
                    appUser = newAppUser;
                }
            }
            var newUserRole = userManager.AddToRoleAsync(appUser, adminRoleName);
            newUserRole.Wait();
        }
    }
}
using Clinic_Web.Models.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Strona.Areas.Identity.Data;
using Strona.Data;
using System;
using System.Threading.Tasks;

namespace Strona
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
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddDbContext<Database_controller>(options =>
                    options.UseLazyLoadingProxies().UseSqlServer(
                        Configuration.GetConnectionString("DBContextConnection")));
            services.AddDefaultIdentity<Patient_account>(options => options.SignIn.RequireConfirmedAccount = false).AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<DBContext>().AddDefaultTokenProviders();

            //Info about Passwords Strength

            services.Configure<IdentityOptions>(options =>
            {
                  // Password settings
                  options.Password.RequireDigit = false;
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 0;

                  // Lockout settings
                  options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                  // User settings
                  options.User.RequireUniqueEmail = true;
            });
            /*
               //The Account Login page's settings
               services.ConfigureApplicationCookie(options =>
               {
                   // Cookies settings
                   options.Cookie.HttpOnly = true;
                   options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                   options.LoginPath = "/Account/Login"; // You can type here you own LoginPath, if you don't set custom path, ASP.NET Core will default to /Account/Login
                   options.LogoutPath = "/Account/Logout"; // You can type here you own LogoutPath, if you don't set custom path, ASP.NET Core will default to /Account/Logout
                   options.AccessDeniedPath = "/Account/AccessDenied"; // You can type you own AccesDeniedPath, if you don't set custom path, ASP.NET Core will default to /Account/AccessDenied;
                   options.SlidingExpiration = true;
               });
               */
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider services)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
           // app.UseBrowserLink();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
            CreateUserRoles(services).Wait();
        }
        private async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            // Initializing custom roles   
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<Patient_account>>();
            string[] roleNames = { "Admin", "Patient", "Doctor" };

            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    //create the roles and seed them to the database
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Assign Admin role to newly registered user
           Patient_account user = await UserManager.FindByEmailAsync("g.zatorski90@wp.pl");
            var User = new Patient_account();
           await UserManager.AddToRoleAsync(User, "Patient");
          //  await UserManager.AddToRoleAsync(user, "Admin");
        }

    }

}

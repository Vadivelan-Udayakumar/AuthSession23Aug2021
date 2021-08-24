using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthSessionDemoAuthorization.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AuthSessionDemoAuthorization
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<IdentityUser, IdentityRole>()
            .AddRoleManager<RoleManager<IdentityRole>>()
            .AddDefaultUI()
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAuthorization(options =>
            {
                //Role based policy
                options.AddPolicy("AdminsOnly", policy => policy.RequireRole("Admin", "SuperAdmin", "AccountAdmin"));

                // Claims based policy
                options.AddPolicy("WithWriteClaimOnly", policy => policy.RequireClaim(ClaimTypes.AuthorizationDecision, 
                    "Write",
                    "Write.All",
                    "ReadWrite",
                    "ReadWrite.All"
                    ));
            });

            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });

            //seed the data
            //createroles(serviceprovider).wait();
        }

        //private async Task CreateRoles(IServiceProvider serviceProvider)
        //{
        //    var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        //    var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        //    string[] roleNames = { "Admin", "Reader", "Editor" };
        //    IdentityResult roleResult;

        //    foreach (var roleName in roleNames)
        //    {
        //        var roleExist = await RoleManager.RoleExistsAsync(roleName);
        //        if (!roleExist)
        //        {
        //            var role = new IdentityRole(roleName);
        //            roleResult = await RoleManager.CreateAsync(role);

        //            if (roleName == "Admin")
        //                RoleManager.AddClaimAsync(role, new Claim(ClaimTypes.AuthorizationDecision, "ReadWrite.All")).Wait();
        //            else if (roleName == "Reader")
        //                RoleManager.AddClaimAsync(role, new Claim(ClaimTypes.AuthorizationDecision, "Read")).Wait();
        //            else if (roleName == "Editor")
        //                RoleManager.AddClaimAsync(role, new Claim(ClaimTypes.AuthorizationDecision, "ReadWrite")).Wait();
        //        }
        //    }

        //    var _admin = await UserManager.FindByEmailAsync("admin@admin.com");
        //    if (_admin == null)
        //    {
        //        var admin = new ApplicationUser
        //        {
        //            UserName = "admin@admin.com",
        //            Email = "admin@admin.com"
        //        };

        //        var createAdmin = await UserManager.CreateAsync(admin, "SomePassword");
        //        if (createAdmin.Succeeded)
        //            await UserManager.AddToRoleAsync(admin, "Admin");
        //    }

        //    var _reader = await UserManager.FindByEmailAsync("reader@reader.com");
        //    if (_reader == null)
        //    {
        //        var reader = new ApplicationUser
        //        {
        //            UserName = "reader@reader.com",
        //            Email = "reader@reader.com"
        //        };

        //        var createreader = await UserManager.CreateAsync(reader, "SomePassword");
        //        if (createreader.Succeeded)
        //            await UserManager.AddToRoleAsync(reader, "Reader");
        //    }

        //    var _editor = await UserManager.FindByEmailAsync("editor@editor.com");
        //    if (_editor == null)
        //    {
        //        var editor = new ApplicationUser
        //        {
        //            UserName = "editor@editor.com",
        //            Email = "editor@editor.com"
        //        };

        //        var createStudent = await UserManager.CreateAsync(editor, "SomePassword");
        //        if (createStudent.Succeeded)
        //            await UserManager.AddToRoleAsync(editor, "Editor");
        //    }
        //}
    }
}

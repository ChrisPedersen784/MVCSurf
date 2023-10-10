using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SurfBoardProject.Data;
using SurfBoardProject.Models;
using SurfBoardProject.Utility;
using System.Globalization;
using Microsoft.AspNetCore.Identity;

namespace SurfBoardProject
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<SurfBoardProjectContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("SurfBoardProjectContext") ?? throw new InvalidOperationException("Connection string 'SurfBoardProjectContext' not found.")));

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                  .AddRoles<IdentityRole>() // Add support for roles
    .AddEntityFrameworkStores<SurfBoardProjectContext>();

           

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<BoardService>();
            builder.Services.AddHttpClient();

            var app = builder.Build();

            //Set the default culture to Danish

            CultureInfo.CurrentCulture = new CultureInfo("da-DK");
            CultureInfo.CurrentUICulture = new CultureInfo("da-DK");


            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                SeedData.Initialize(services);
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication(); ;

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                // create roles
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var roles = new[] { "Admin", "Customer", "Administration", "Guest" };
                foreach (var role in roles)
                {

                    // check if role exists and if they dont create them
                    //Here you can manually add a new user role to the DB
                    if (!roleManager.RoleExistsAsync("Customer").Result)
                    {
                        roleManager.CreateAsync(new IdentityRole("Customer")).Wait();
                    }

                    //if (!roleManager.RoleExistsAsync("Admin").Result)
                    //{
                    //    roleManager.CreateAsync(new IdentityRole("Admin")).Wait();
                    //}
                }

                // SeedData.Initialize(services); 
            }

            app.Run();
        }


    }
}
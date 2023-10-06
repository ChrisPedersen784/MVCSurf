using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SurfBoardProject.Data;

namespace RentalWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<SurfBoardProjectContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("SurfBoardProjectContext") ?? throw new InvalidOperationException("Connection string 'SurfBoardProjectContext' not found.")));

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                 .AddRoles<IdentityRole>() // Add support for roles
   .AddEntityFrameworkStores<SurfBoardProjectContext>();

            builder.Services.AddScoped<SurfBoardProjectContext>();
           

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                // create roles
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var roles = new[] { "Admin", "Customer", "Administration" };
                foreach (var role in roles)
                {

                    // check if role exists and if they dont create them
                    //Here you can manually add a new user role to the DB
                    if (!roleManager.RoleExistsAsync("Admin").Result)
                    {
                        roleManager.CreateAsync(new IdentityRole("Admin")).Wait();
                    }

                    if (!roleManager.RoleExistsAsync("Admin").Result)
                    {
                        roleManager.CreateAsync(new IdentityRole("Admin")).Wait();
                    }
                }

                // SeedData.Initialize(services); 
            }

            app.Run();
        }


    }
}
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using SurfBoardProject.Data;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            // Add services to the container.
            builder.Services.AddApiVersioning(options =>
            {
                //options.ReportApiVersions = true;: This option enables reporting
                //of API versions in response headers and documentation, making it easier
                //for clients to understand the available API versions.
                options.ReportApiVersions = true;

                //if i dont specify a version the 1.0 is default
                //options.DefaultApiVersion = new ApiVersion(1, 0);:
                //This sets the default API version to 1.0. If a version is not
                //specified in the request, this version will be assumed.
                options.DefaultApiVersion = new ApiVersion(1, 0);

                //options.AssumeDefaultVersionWhenUnspecified = true;:
                //This option tells the system to assume the default API version (1.0)
                //when a version is not explicitly mentioned in the request.
                options.AssumeDefaultVersionWhenUnspecified = true;

                //options.ApiVersionReader = new QueryStringApiVersionReader("api-version");:
                //This line configures the API version reader to use query string versioning. In your requests,
                //you can specify the API version using the api-version query parameter (e.g., ?api-version=1.0).
                options.ApiVersionReader = new QueryStringApiVersionReader("api-version");
            });


            builder.Services.AddDbContext<SurfBoardProjectContext>(options =>
          options.UseSqlServer(builder.Configuration.GetConnectionString("SurfBoardProjectContext") ?? throw new InvalidOperationException("Connection string 'SurfBoardProjectContext' not found.")));

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>() // Add support for roles
  .AddEntityFrameworkStores<SurfBoardProjectContext>();


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
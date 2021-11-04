using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SportsStore.Models;

namespace SportsStore
{
    public class Startup
    {
        private IConfiguration Configuration;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(); 

            // The name of the connection string is passed in to the context by calling a method on a DbContextOptionsBuilder object. For local development, the ASP.NET Core configuration system reads the connection string from the appsettings.json file.
            var x = Configuration.GetConnectionString("ConnectionString");
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(x));
            services.AddScoped<IProductRepository, EFProductRepository>();
            services.AddScoped<Cart>(SessionCart.GetCart);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMemoryCache();
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.UseSession needs to be before one of the statements below. Otherwise you'll get an error. A little bit searching found the solution
            app.UseSession();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapRazorPages(); // not sure what this one does for now.
                endpoints.MapControllerRoute("catPage", "Products/{category}/Page{page:int}", new { Controller = "Product", action = "List" });
                endpoints.MapControllerRoute("Page", "Products/{page:int}", new { Controller = "Product", action = "List" });
                endpoints.MapControllerRoute("category", "Products/{category}", new { Controller = "Product", action = "List" });
                endpoints.MapControllerRoute("pagination", "Products/Page{page:int}", new { Controller = "Product", action = "List" }); // note 'page' is the name of the variable in the parameter. They have to match.
                endpoints.MapControllerRoute(name: "default", pattern: "{controller=Product}/{action=List}/{id?}");
            });

            app.UseStatusCodePages();
            app.UseStaticFiles();
            SeedData.EnsurePopulated(app);
        }
    }
}

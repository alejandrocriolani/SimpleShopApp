using Almacen.Models;
using Almacen.Models.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Almacen
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; set; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AlmacenDbContext>(options =>
                options.UseSqlServer(
                    Configuration["Data:AlmacenIdentity:ConnectionString"]));

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<AlmacenDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddMvc();
            services.AddMemoryCache();
            services.AddSession();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IOrderRepository, EFOrderRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(
                              IApplicationBuilder app, 
                              IHostingEnvironment env)
        {
            app.UseStatusCodePages();
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseSession();
            app.UseMvcWithDefaultRoute();
            await CreateRoles(app.ApplicationServices);
        }

        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            RoleManager<IdentityRole> roleManager =
                serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            UserManager<User> userManager =
                serviceProvider.GetRequiredService<UserManager<User>>();

            string[] roleNames = { "Admin", "Employee", "Client", "Supplier" };
            IdentityResult roleResult;
            foreach(string role in roleNames)
            {
                bool roleExist = await roleManager.RoleExistsAsync(role);
                if(!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            if(await roleManager.RoleExistsAsync("Admin, Employee, Client, Supplier"))
            {
                IdentityRole role = await roleManager.FindByNameAsync("Admin, Employee, Client, Supplier");
                await roleManager.DeleteAsync(role);
            }
        }

    }
}

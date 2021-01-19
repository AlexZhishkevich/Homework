using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OnlineStore.Contracts.Interfaces;
using OnlineStore.EntityFrameworkDataProvider;
using OnlineStore.EntityFrameworkDataProvider.Repositories;

namespace OnlineStore.WebApp
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
            ConfigureEntityFrameworkDataProvider(services);

            services.AddControllersWithViews();
            services.AddDistributedMemoryCache();
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
            app.UseSession();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void ConfigureEntityFrameworkDataProvider(IServiceCollection services)
        {
            services.AddDbContext<OnlineStoreContext>(optionsBuilder =>
                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            var options = new DbContextOptionsBuilder<OnlineStoreContext>().UseSqlServer(Configuration.GetConnectionString("DefaultConnection")).Options;

            services.AddSingleton<ICatalogRepository>(new EntityFrameworkCatalogRepository(options));
            services.AddSingleton<IGoodRepository>(new EntityFrameworkGoodRepository(options));
        }
    }
}
